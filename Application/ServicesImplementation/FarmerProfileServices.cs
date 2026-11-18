using Application.Dtos;
using Application.RepositoryInterface;
using Application.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServicesImplementation
{
    public class FarmerProfileServices : IFarmerProfileServices
    {
        private readonly IFarmerProfileRepository _farmerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FarmerProfileServices(
            IFarmerProfileRepository farmerRepository,
            IUnitOfWork unitOfWork)
        {
            _farmerRepository = farmerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<FarmerDto>> GetProfileAsync(Guid userId)
        {
            var farmer = await _farmerRepository.GetByUserIdAsync(userId);

            if (farmer == null)
            {
                return BaseResponse<FarmerDto>.Failure(
                    "Farmer profile was not found.");
            }

            var response = new FarmerDto
            {
                UserId = farmer.UserId,
                FarmName = farmer.FarmName,
                FarmAddress = farmer.FarmAddress,
                State = farmer.State,
                IsVerified = farmer.IsVerified,

                FirstName = farmer.User.FirstName,
                LastName = farmer.User.LastName,
                UserName = farmer.User.UserName,
                Email = farmer.User.Email,
                PhoneNumber = farmer.User.PhoneNumber
            };

            return BaseResponse<FarmerDto>.Success(
                "Farmer profile retrieved successfully.",
                response);
        }

        public async Task<BaseResponse<FarmerDto>> UpdateProfileAsync(
            Guid userId,
            UpdateFarmerProfileRequestModel model)
        {
            try
            {
                var farmer = await _farmerRepository.GetByUserIdAsync(userId);

                if (farmer == null)
                {
                    return BaseResponse<FarmerDto>.Failure(
                        "Farmer profile was not found.");
                }

                farmer.UpdateFarmInformation(
                    model.FarmName,
                    model.FarmAddress,
                    model.State);

                _farmerRepository.Update(farmer);

                await _unitOfWork.SaveChangesAsync();

                var response = new FarmerDto
                {
                    UserId = farmer.UserId,
                    FarmName = farmer.FarmName,
                    FarmAddress = farmer.FarmAddress,
                    State = farmer.State,
                    IsVerified = farmer.IsVerified,

                    FirstName = farmer.User.FirstName,
                    LastName = farmer.User.LastName,
                    UserName = farmer.User.UserName,
                    Email = farmer.User.Email,
                    PhoneNumber = farmer.User.PhoneNumber
                };

                return BaseResponse<FarmerDto>.Success(
                    "Farmer profile updated successfully.",
                    response);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<FarmerDto>.Failure(ex.Message);
            }
        }
    }
}
