using Application.Dtos;

namespace Application.ServicesInterface
{
    public interface ICropServices
    {
        Task<BaseResponse<List<CropDto>>> GetAllAsync();
        Task<BaseResponse<CreateCropResponseModel>> CreateAsync(CreateCropRequestModel model);
        Task<BaseResponse<CropDto>> UpdateAsync(Guid id, UpdateCropRequestModel model);
    }
}
