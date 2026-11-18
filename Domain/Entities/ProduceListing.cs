using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ProduceListing : BaseEntity
    {
        public Guid FarmerProfileId { get; private set; }
        public FarmerProfile FarmerProfile { get; private set; } = null!;
        public Guid CropId { get; private set; }
        public Crop Crop { get; private set; } = null!;
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal PricePerUnit { get; private set; }
        public string Unit { get; private set; } = string.Empty;
        public decimal TotalQuantity { get; private set; }
        public decimal ReservedQuantity { get; private set; }
        public DateTime AvailableFrom { get; private set; }
        public ListingStatus Status { get; private set; }

        public decimal AvailableQuantity => TotalQuantity - ReservedQuantity;
        private ProduceListing() { }

        public ProduceListing(Guid farmerProfileId, Guid cropId, string title, string description,
            decimal pricePerUnit, string unit, decimal quantity, DateTime availableFrom)
        {
            if (farmerProfileId == Guid.Empty || cropId == Guid.Empty) throw new ArgumentException("Farmer and crop are required.");
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(unit)) throw new ArgumentException("Title and unit are required.");
            if (pricePerUnit <= 0 || quantity <= 0) throw new ArgumentException("Price and quantity must be greater than zero.");
            FarmerProfileId = farmerProfileId;
            CropId = cropId;
            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
            PricePerUnit = pricePerUnit;
            Unit = unit.Trim();
            TotalQuantity = quantity;
            AvailableFrom = availableFrom;
            Status = ListingStatus.Available;
        }

        public void Reserve(decimal quantity)
        {
            if (Status == ListingStatus.Cancelled || Status == ListingStatus.SoldOut)
                throw new InvalidOperationException("This listing is not available for reservation.");
            if (quantity <= 0 || quantity > AvailableQuantity)
                throw new InvalidOperationException("Requested quantity is not available.");
            ReservedQuantity += quantity;
            Status = AvailableQuantity == 0 ? ListingStatus.SoldOut : ListingStatus.PartiallyReserved;
            MarkAsUpdated();
        }

        public void ReleaseReservation(decimal quantity)
        {
            if (quantity <= 0 || quantity > ReservedQuantity) throw new InvalidOperationException("Invalid quantity to release.");
            ReservedQuantity -= quantity;
            Status = ReservedQuantity == 0 ? ListingStatus.Available : ListingStatus.PartiallyReserved;
            MarkAsUpdated();
        }

        public void Cancel()
        {
            if (ReservedQuantity > 0) throw new InvalidOperationException("Cannot cancel a listing with active reservations.");
            Status = ListingStatus.Cancelled;
            MarkAsUpdated();
        }
    }
}
