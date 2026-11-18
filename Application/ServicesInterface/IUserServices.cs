
using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServicesInterface
{
    
        public interface IUserServices
        {
            Task<BaseResponse<CreateBuyerProfileResponseModel>>
                RegisterBuyerAsync(CreateBuyerProfileRequestModel model);

            Task<BaseResponse<CreateFarmerProfileResponseModel>>
                RegisterFarmerAsync(CreateFarmerProfileRequestModel model);

            Task<BaseResponse<LoginResponseModel>>
                LoginAsync(LoginRequestModel model);
        }
    
}
