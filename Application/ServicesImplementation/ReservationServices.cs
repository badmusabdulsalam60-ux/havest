using Application.Dtos;
using Application.RepositoryInterface;
using Application.ServicesInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.ServicesImplementation
{
    public class ReservationServices : IReservationServices
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IProduceListingRepository _listingRepository;
        private readonly IFarmerProfileRepository _farmerRepository;
        private readonly IBuyerProfileRepository _buyerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReservationServices(
            IReservationRepository reservationRepository,
            IProduceListingRepository listingRepository,
            IFarmerProfileRepository farmerRepository,
            IBuyerProfileRepository buyerRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _listingRepository = listingRepository;
            _farmerRepository = farmerRepository;
            _buyerRepository = buyerRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        // Buyer reserves produce. The quantity is held immediately and released if the farmer rejects.
        public async Task<BaseResponse<CreateReservationResponseModel>> CreateAsync(
            Guid userId, CreateReservationRequestModel model)
        {
            try
            {
                var buyer = await _buyerRepository.GetByUserIdAsync(userId);

                if (buyer == null)
                {
                    return BaseResponse<CreateReservationResponseModel>.Failure("Only buyers can make reservations.");
                }

                var listing = await _listingRepository.GetByIdWithDetailsAsync(model.ProduceListingId);

                if (listing == null)
                {
                    return BaseResponse<CreateReservationResponseModel>.Failure("Listing was not found.");
                }

                listing.Reserve(model.Quantity);

                var reservation = new Reservation(listing.Id, buyer.Id, model.Quantity);

                await _reservationRepository.AddAsync(reservation);
                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<CreateReservationResponseModel>.Success(
                    "Reservation created. Waiting for the farmer to respond.",
                    new CreateReservationResponseModel { Id = reservation.Id });
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<CreateReservationResponseModel>.Failure(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BaseResponse<CreateReservationResponseModel>.Failure(ex.Message);
            }
        }

        // Farmer accepts (an order is created) or rejects (quantity goes back to the listing)
        public async Task<BaseResponse<ReservationDto>> RespondAsync(Guid userId, Guid reservationId, bool accept)
        {
            try
            {
                var farmer = await _farmerRepository.GetByUserIdAsync(userId);

                if (farmer == null)
                {
                    return BaseResponse<ReservationDto>.Failure("Only farmers can respond to reservations.");
                }

                var reservation = await _reservationRepository.GetByIdWithDetailsAsync(reservationId);

                if (reservation == null)
                {
                    return BaseResponse<ReservationDto>.Failure("Reservation was not found.");
                }

                if (reservation.ProduceListing.FarmerProfileId != farmer.Id)
                {
                    return BaseResponse<ReservationDto>.Failure("This reservation does not belong to your listing.");
                }

                if (await _reservationRepository.HasOrderAsync(reservation.Id))
                {
                    return BaseResponse<ReservationDto>.Failure("This reservation already has an order.");
                }

                if (accept)
                {
                    reservation.Accept();
                    await _orderRepository.AddAsync(new Order(reservation.Id));
                }
                else
                {
                    reservation.Reject();
                    reservation.ProduceListing.ReleaseReservation(reservation.Quantity);
                }

                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<ReservationDto>.Success(
                    accept ? "Reservation accepted and order created." : "Reservation rejected.",
                    MapToDto(reservation));
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<ReservationDto>.Failure(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BaseResponse<ReservationDto>.Failure(ex.Message);
            }
        }

        // Farmer sees reservations on his listings, buyer sees the reservations he made
        public async Task<BaseResponse<List<ReservationDto>>> GetMineAsync(Guid userId)
        {
            var farmer = await _farmerRepository.GetByUserIdAsync(userId);

            if (farmer != null)
            {
                var farmerList = await _reservationRepository.ForFarmer(farmer.Id)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                return BaseResponse<List<ReservationDto>>.Success(
                    "Reservations retrieved successfully.",
                    farmerList.Select(MapToDto).ToList());
            }

            var buyer = await _buyerRepository.GetByUserIdAsync(userId);

            if (buyer != null)
            {
                var buyerList = await _reservationRepository.ForBuyer(buyer.Id)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                return BaseResponse<List<ReservationDto>>.Success(
                    "Reservations retrieved successfully.",
                    buyerList.Select(MapToDto).ToList());
            }

            return BaseResponse<List<ReservationDto>>.Failure("No farmer or buyer profile was found for this user.");
        }

        private static ReservationDto MapToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                ProduceListingId = reservation.ProduceListingId,
                ListingTitle = reservation.ProduceListing.Title,
                Quantity = reservation.Quantity,
                Status = reservation.Status,
                BuyerProfileId = reservation.BuyerProfileId,
                FarmerProfileId = reservation.ProduceListing.FarmerProfileId,
                CreatedAt = reservation.CreatedAt
            };
        }
    }
}
