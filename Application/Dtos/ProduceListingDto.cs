using Domain.Enum;
using System;

namespace Application.Dtos
{
    public class ProduceListingDto
    {
        public Guid Id { get; set; }
        public Guid FarmerProfileId { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public Guid CropId { get; set; }
        public string CropName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal TotalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public DateTime AvailableFrom { get; set; }
        public ListingStatus Status { get; set; }
    }

    public class CreateProduceListingRequestModel
    {
        public Guid CropId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime AvailableFrom { get; set; }
    }

    public class CreateProduceListingResponseModel
    {
        public Guid Id { get; set; }
    }
}
