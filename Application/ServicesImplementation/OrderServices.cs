using Application.Dtos;
using Application.RepositoryInterface;
using Application.ServicesInterface;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.ServicesImplementation
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFarmerProfileRepository _farmerRepository;
        private readonly IBuyerProfileRepository _buyerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderServices(
            IOrderRepository orderRepository,
            IFarmerProfileRepository farmerRepository,
            IBuyerProfileRepository buyerRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _farmerRepository = farmerRepository;
            _buyerRepository = buyerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<OrderDto>> GetByIdAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(orderId);

            if (order == null)
            {
                return BaseResponse<OrderDto>.Failure("Order was not found.");
            }

            var isFarmer = await IsOrderFarmerAsync(userId, order);
            var isBuyer = await IsOrderBuyerAsync(userId, order);

            if (!isFarmer && !isBuyer)
            {
                return BaseResponse<OrderDto>.Failure("You do not have access to this order.");
            }

            return BaseResponse<OrderDto>.Success("Order retrieved successfully.", MapToDto(order));
        }

        public async Task<BaseResponse<List<OrderDto>>> GetMineAsync(Guid userId)
        {
            var farmer = await _farmerRepository.GetByUserIdAsync(userId);

            if (farmer != null)
            {
                var farmerOrders = await _orderRepository.ForFarmer(farmer.Id)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                return BaseResponse<List<OrderDto>>.Success(
                    "Orders retrieved successfully.",
                    farmerOrders.Select(MapToDto).ToList());
            }

            var buyer = await _buyerRepository.GetByUserIdAsync(userId);

            if (buyer != null)
            {
                var buyerOrders = await _orderRepository.ForBuyer(buyer.Id)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                return BaseResponse<List<OrderDto>>.Success(
                    "Orders retrieved successfully.",
                    buyerOrders.Select(MapToDto).ToList());
            }

            return BaseResponse<List<OrderDto>>.Failure("No farmer or buyer profile was found for this user.");
        }

        // Farmer: Preparing, ReadyForPickup, Delivered, Cancelled.   Buyer: Completed.
        public async Task<BaseResponse<OrderDto>> UpdateStatusAsync(Guid userId, Guid orderId, OrderStatus status)
        {
            try
            {
                var order = await _orderRepository.GetByIdWithDetailsAsync(orderId);

                if (order == null)
                {
                    return BaseResponse<OrderDto>.Failure("Order was not found.");
                }

                var isFarmer = await IsOrderFarmerAsync(userId, order);
                var isBuyer = await IsOrderBuyerAsync(userId, order);

                if (!isFarmer && !isBuyer)
                {
                    return BaseResponse<OrderDto>.Failure("You do not have access to this order.");
                }

                var allowed = isFarmer
                    ? new List<OrderStatus> { OrderStatus.Preparing, OrderStatus.ReadyForPickup, OrderStatus.Delivered, OrderStatus.Cancelled }
                    : new List<OrderStatus> { OrderStatus.Completed };

                if (!allowed.Contains(status))
                {
                    return BaseResponse<OrderDto>.Failure($"You are not allowed to set the order to '{status}'.");
                }

                order.UpdateStatus(status);

                if (status == OrderStatus.Cancelled)
                {
                    // give the quantity back to the listing
                    order.Reservation.Cancel();
                    order.Reservation.ProduceListing.ReleaseReservation(order.Reservation.Quantity);
                }

                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<OrderDto>.Success("Order status updated successfully.", MapToDto(order));
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<OrderDto>.Failure(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BaseResponse<OrderDto>.Failure(ex.Message);
            }
        }

        private async Task<bool> IsOrderFarmerAsync(Guid userId, Order order)
        {
            var farmer = await _farmerRepository.GetByUserIdAsync(userId);
            return farmer != null && order.Reservation.ProduceListing.FarmerProfileId == farmer.Id;
        }

        private async Task<bool> IsOrderBuyerAsync(Guid userId, Order order)
        {
            var buyer = await _buyerRepository.GetByUserIdAsync(userId);
            return buyer != null && order.Reservation.BuyerProfileId == buyer.Id;
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = order.Status,
                Quantity = order.Reservation.Quantity,
                ListingTitle = order.Reservation.ProduceListing.Title,
                BuyerProfileId = order.Reservation.BuyerProfileId,
                FarmerProfileId = order.Reservation.ProduceListing.FarmerProfileId,
                CreatedAt = order.CreatedAt
            };
        }
    }
}
