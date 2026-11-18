using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServicesInterface
{
    public interface IBuyerProfileServices
    {
        Task<BaseResponse<BuyerDto>> GetProfileAsync(Guid userId);

        Task<BaseResponse<BuyerDto>> UpdateProfileAsync(
            Guid userId,
            UpdateBuyerProfileRequestModel model);
    }
}
