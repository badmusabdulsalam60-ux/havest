using Application.Dtos;
using Domain.Enum;

namespace Application.ServicesInterface
{
    public interface IOrderServices
    {
        Task<BaseResponse<OrderDto>> GetByIdAsync(Guid userId, Guid orderId);
        Task<BaseResponse<List<OrderDto>>> GetMineAsync(Guid userId);
        Task<BaseResponse<OrderDto>> UpdateStatusAsync(Guid userId, Guid orderId, OrderStatus status);
    }
}
