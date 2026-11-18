using Application.Dtos;
using Application.RepositoryInterface;
using Application.ServicesInterface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.ServicesImplementation
{
    public class CropServices : ICropServices
    {
        private readonly ICropRepository _cropRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CropServices(ICropRepository cropRepository, IUnitOfWork unitOfWork)
        {
            _cropRepository = cropRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<List<CropDto>>> GetAllAsync()
        {
            var crops = await _cropRepository.GetAll();

            var response = crops
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new CropDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Category = c.Category,
                    Description = c.Description,
                    IsActive = c.IsActive
                })
                .ToList();

            return BaseResponse<List<CropDto>>.Success("Crops retrieved successfully.", response);
        }

        public async Task<BaseResponse<CreateCropResponseModel>> CreateAsync(CreateCropRequestModel model)
        {
            try
            {
                if (await _cropRepository.NameExistsAsync(model.Name.Trim()))
                {
                    return BaseResponse<CreateCropResponseModel>.Failure("A crop with this name already exists.");
                }

                var crop = new Crop(model.Name, model.Category, model.Description);

                await _cropRepository.AddAsync(crop);
                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<CreateCropResponseModel>.Success(
                    "Crop created successfully.",
                    new CreateCropResponseModel { Id = crop.Id });
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<CreateCropResponseModel>.Failure(ex.Message);
            }
        }

        public async Task<BaseResponse<CropDto>> UpdateAsync(Guid id, UpdateCropRequestModel model)
        {
            try
            {
                var crop = await _cropRepository.GetByIdAsync(id);

                if (crop == null)
                {
                    return BaseResponse<CropDto>.Failure("Crop was not found.");
                }

                var nameTaken = await _cropRepository.Query().AnyAsync(x =>
                    x.Id != id && !x.IsDeleted && x.Name.ToLower() == model.Name.Trim().ToLower());

                if (nameTaken)
                {
                    return BaseResponse<CropDto>.Failure("A crop with this name already exists.");
                }

                crop.Update(model.Name, model.Category, model.Description);

                _cropRepository.Update(crop);
                await _unitOfWork.SaveChangesAsync();

                var response = new CropDto
                {
                    Id = crop.Id,
                    Name = crop.Name,
                    Category = crop.Category,
                    Description = crop.Description,
                    IsActive = crop.IsActive
                };

                return BaseResponse<CropDto>.Success("Crop updated successfully.", response);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<CropDto>.Failure(ex.Message);
            }
        }
    }
}
