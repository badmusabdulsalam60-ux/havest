using Application.Dtos;
using Application.RepositoryInterface;
using Application.ServicesInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.ServicesImplementation
{
    public class ProduceListingServices : IProduceListingServices
    {
        private readonly IProduceListingRepository _listingRepository;
        private readonly IFarmerProfileRepository _farmerRepository;
        private readonly ICropRepository _cropRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProduceListingServices(
            IProduceListingRepository listingRepository,
            IFarmerProfileRepository farmerRepository,
            ICropRepository cropRepository,
            IUnitOfWork unitOfWork)
        {
            _listingRepository = listingRepository;
            _farmerRepository = farmerRepository;
            _cropRepository = cropRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CreateProduceListingResponseModel>> CreateAsync(
            Guid userId, CreateProduceListingRequestModel model)
        {
            try
            {
                var farmer = await _farmerRepository.GetByUserIdAsync(userId);

                if (farmer == null)
                {
                    return BaseResponse<CreateProduceListingResponseModel>.Failure("Only farmers can create listings.");
                }

                var crop = await _cropRepository.GetByIdAsync(model.CropId);

                if (crop == null || !crop.IsActive)
                {
                    return BaseResponse<CreateProduceListingResponseModel>.Failure("Crop was not found.");
                }

                var listing = new ProduceListing(
                    farmer.Id,
                    crop.Id,
                    model.Title,
                    model.Description,
                    model.PricePerUnit,
                    model.Unit,
                    model.Quantity,
                    model.AvailableFrom);

                await _listingRepository.AddAsync(listing);
                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<CreateProduceListingResponseModel>.Success(
                    "Produce listing created successfully.",
                    new CreateProduceListingResponseModel { Id = listing.Id });
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<CreateProduceListingResponseModel>.Failure(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ProduceListingDto>>> GetAvailableAsync(string? search)
        {
            var listings = await _listingRepository
                .SearchAvailable(search)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var response = listings.Select(MapToDto).ToList();

            return BaseResponse<List<ProduceListingDto>>.Success("Listings retrieved successfully.", response);
        }

        public async Task<BaseResponse<ProduceListingDto>> GetByIdAsync(Guid listingId)
        {
            var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId);

            if (listing == null)
            {
                return BaseResponse<ProduceListingDto>.Failure("Listing was not found.");
            }

            return BaseResponse<ProduceListingDto>.Success("Listing retrieved successfully.", MapToDto(listing));
        }

        private static ProduceListingDto MapToDto(ProduceListing listing)
        {
            return new ProduceListingDto
            {
                Id = listing.Id,
                FarmerProfileId = listing.FarmerProfileId,
                FarmName = listing.FarmerProfile.FarmName,
                CropId = listing.CropId,
                CropName = listing.Crop.Name,
                Title = listing.Title,
                Description = listing.Description,
                PricePerUnit = listing.PricePerUnit,
                Unit = listing.Unit,
                TotalQuantity = listing.TotalQuantity,
                AvailableQuantity = listing.AvailableQuantity,
                AvailableFrom = listing.AvailableFrom,
                Status = listing.Status
            };
        }
    }
}
