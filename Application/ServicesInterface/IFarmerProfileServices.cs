using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServicesInterface
{
    public interface IFarmerProfileServices
    {
        Task<BaseResponse<FarmerDto>> GetProfileAsync(Guid userId);

        Task<BaseResponse<FarmerDto>> UpdateProfileAsync(
            Guid userId,
            UpdateFarmerProfileRequestModel model);
    }
}
