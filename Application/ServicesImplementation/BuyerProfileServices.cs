using Application.Dtos;
using Application.RepositoryInterface;
using Application.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServicesImplementation
{
    public class BuyerProfileServices : IBuyerProfileServices
    {
        private readonly IBuyerProfileRepository _buyerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BuyerProfileServices(
            IBuyerProfileRepository buyerRepository,
            IUnitOfWork unitOfWork)
        {
            _buyerRepository = buyerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<BuyerDto>> GetProfileAsync(Guid userId)
        {
            var buyer = await _buyerRepository.GetByUserIdAsync(userId);

            if (buyer == null)
            {
                return BaseResponse<BuyerDto>.Failure(
                    "Buyer profile was not found.");
            }

            var response = new BuyerDto
            {
                UserId = buyer.UserId,
                BuyerType = buyer.BuyerType,
                BusinessName = buyer.BusinessName,

                FirstName = buyer.User.FirstName,
                LastName = buyer.User.LastName,
                UserName = buyer.User.UserName,
                Email = buyer.User.Email,
                PhoneNumber = buyer.User.PhoneNumber
            };

            return BaseResponse<BuyerDto>.Success(
                "Buyer profile retrieved successfully.",
                response);
        }

        public async Task<BaseResponse<BuyerDto>> UpdateProfileAsync(
            Guid userId,
            UpdateBuyerProfileRequestModel model)
        {
            try
            {
                var buyer = await _buyerRepository.GetByUserIdAsync(userId);

                if (buyer == null)
                {
                    return BaseResponse<BuyerDto>.Failure(
                        "Buyer profile was not found.");
                }

                buyer.UpdateBuyerInformation(
                    model.BuyerType,
                    model.BusinessName);

                _buyerRepository.Update(buyer);

                await _unitOfWork.SaveChangesAsync();

                var response = new BuyerDto
                {
                    UserId = buyer.UserId,
                    BuyerType = buyer.BuyerType,
                    BusinessName = buyer.BusinessName,

                    FirstName = buyer.User.FirstName,
                    LastName = buyer.User.LastName,
                    UserName = buyer.User.UserName,
                    Email = buyer.User.Email,
                    PhoneNumber = buyer.User.PhoneNumber
                };

                return BaseResponse<BuyerDto>.Success(
                    "Buyer profile updated successfully.",
                    response);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<BuyerDto>.Failure(ex.Message);
            }
        }
    }
}
