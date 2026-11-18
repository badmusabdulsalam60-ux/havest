using Domain.Enum;
using System;

namespace Application.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal Quantity { get; set; }
        public string ListingTitle { get; set; } = string.Empty;
        public Guid BuyerProfileId { get; set; }
        public Guid FarmerProfileId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateOrderStatusRequestModel
    {
        public OrderStatus Status { get; set; }
    }
}
