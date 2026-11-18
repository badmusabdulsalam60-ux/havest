using Domain.Enum;
using System;

namespace Application.Dtos
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid ProduceListingId { get; set; }
        public string ListingTitle { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public ReservationStatus Status { get; set; }
        public Guid BuyerProfileId { get; set; }
        public Guid FarmerProfileId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReservationRequestModel
    {
        public Guid ProduceListingId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class CreateReservationResponseModel
    {
        public Guid Id { get; set; }
    }
}
