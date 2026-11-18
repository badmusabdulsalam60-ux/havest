
using Application.Dtos;
using Application.RepositoryInterface;
using Application.ServicesInterface;
using Domain.Entities;


namespace Application.ServicesImplementation
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IFarmerProfileRepository _farmerRepository;
        private readonly IBuyerProfileRepository _buyerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IFarmerProfileRepository farmerRepository,
            IBuyerProfileRepository buyerRepository,
            IUnitOfWork unitOfWork,
            ITokenService tokenServices)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _farmerRepository = farmerRepository;
            _buyerRepository = buyerRepository;
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }

        public async Task<BaseResponse<CreateFarmerProfileResponseModel>>
            RegisterFarmerAsync(CreateFarmerProfileRequestModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
                {
                    return BaseResponse<CreateFarmerProfileResponseModel>
                        .Failure("Password must be at least 6 characters.");
                }

                var existingEmail =
                    await _userRepository.GetByEmailAsync(model.Email.Trim());

                if (existingEmail != null)
                {
                    return BaseResponse<CreateFarmerProfileResponseModel>
                        .Failure("Email already exists.");
                }

                var existingUsername =
                    await _userRepository.EmailOrUsernameExistsAsync(model.Email,  model.UserName);

                if (existingUsername)
                {
                    return BaseResponse<CreateFarmerProfileResponseModel>
                        .Failure("Username already exists.");
                }

                var userRole = await _roleRepository.GetByNameAsync("Farmer");

                if (userRole == null)
                {
                    return BaseResponse<CreateFarmerProfileResponseModel>
                        .Failure("User role was not found.");
                }

                var user = new User(
                    model.FirstName,
                    model.LastName,
                    model.UserName,
                    model.Email,
                    model.Password,
                    model.PhoneNumber,
                    userRole.Id
                );

                var farmerProfile = new FarmerProfile(
                    user.Id,
                    model.FarmName,
                    model.FarmAddress,
                    model.State
                );

                await _userRepository.AddAsync(user);
                await _farmerRepository.AddAsync(farmerProfile);

                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<CreateFarmerProfileResponseModel>
                    .Success(
                        "Farmer account created successfully.",
                        new CreateFarmerProfileResponseModel
                        {
                            Id = farmerProfile.Id
                        });
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<CreateFarmerProfileResponseModel>
                    .Failure(ex.Message);
            }
        }

        public async Task<BaseResponse<CreateBuyerProfileResponseModel>>
            RegisterBuyerAsync(CreateBuyerProfileRequestModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
                {
                    return BaseResponse<CreateBuyerProfileResponseModel>
                        .Failure("Password must be at least 6 characters.");
                }

                var existingEmail =
                    await _userRepository.GetByEmailAsync(model.Email.Trim());

                if (existingEmail != null)
                {
                    return BaseResponse<CreateBuyerProfileResponseModel>
                        .Failure("Email already exists.");
                }

                var existingUsername =
                    await _userRepository.EmailOrUsernameExistsAsync(model.Email, model.UserName);


                if (existingUsername)
                {
                    return BaseResponse<CreateBuyerProfileResponseModel>
                        .Failure("Username already exists.");
                }

                var userRole = await _roleRepository.GetByNameAsync("Buyer");

                if (userRole == null)
                {
                    return BaseResponse<CreateBuyerProfileResponseModel>
                        .Failure("User role was not found.");
                }

                var user = new User(
                    model.FirstName,
                    model.LastName,
                    model.UserName,
                    model.Email,
                    model.Password,
                    model.PhoneNumber,
                    userRole.Id
                );

                var buyerProfile = new BuyerProfile(
                    user.Id,
                    model.BuyerType,
                    model.BusinessName
                );

                await _userRepository.AddAsync(user);
                await _buyerRepository.AddAsync(buyerProfile);

                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<CreateBuyerProfileResponseModel>
                    .Success(
                        "Buyer account created successfully.",
                        new CreateBuyerProfileResponseModel
                        {
                            Id = buyerProfile.Id
                        });
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<CreateBuyerProfileResponseModel>
                    .Failure(ex.Message);
            }
        }

        public async Task<BaseResponse<LoginResponseModel>>
            LoginAsync(LoginRequestModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email.Trim());

            if (user == null)
            {
                return BaseResponse<LoginResponseModel>
                    .Failure("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                return BaseResponse<LoginResponseModel>
                    .Failure("Your account is inactive.");
            }

            var isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    model.Password,
                    user.PasswordHash
                );

            if (!isPasswordValid)
            {
                return BaseResponse<LoginResponseModel>
                    .Failure("Invalid email or password.");
            }

            if (user.Role == null)
            {
                return BaseResponse<LoginResponseModel>
                    .Failure("User role could not be loaded.");
            }

            var token = _tokenServices.CreateToken(user);

            var response = new LoginResponseModel
            {
                Token = token,
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Role = user.Role.Name
            };

            return BaseResponse<LoginResponseModel>
                .Success("Login successful.", response);
        }
    }
}