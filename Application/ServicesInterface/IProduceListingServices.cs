using Application.Dtos;

namespace Application.ServicesInterface
{
    public interface IProduceListingServices
    {
        Task<BaseResponse<CreateProduceListingResponseModel>> CreateAsync(Guid userId, CreateProduceListingRequestModel model);
        Task<BaseResponse<List<ProduceListingDto>>> GetAvailableAsync(string? search);
        Task<BaseResponse<ProduceListingDto>> GetByIdAsync(Guid listingId);
    }
}
