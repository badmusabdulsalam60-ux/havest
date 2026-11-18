using Application.Dtos;

namespace Application.ServicesInterface
{
    public interface IReservationServices
    {
        Task<BaseResponse<CreateReservationResponseModel>> CreateAsync(Guid userId, CreateReservationRequestModel model);
        Task<BaseResponse<ReservationDto>> RespondAsync(Guid userId, Guid reservationId, bool accept);
        Task<BaseResponse<List<ReservationDto>>> GetMineAsync(Guid userId);
    }
}
