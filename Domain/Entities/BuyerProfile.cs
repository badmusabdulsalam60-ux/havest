using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class BuyerProfile : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public BuyerType BuyerType { get; private set; }
        public string? BusinessName { get; private set; }

        private BuyerProfile() { }

        public BuyerProfile(Guid userId, BuyerType buyerType, string? businessName)
        {
            if (userId == Guid.Empty) throw new ArgumentException("User is required.");
            UserId = userId;
            UpdateBuyerInformation(buyerType, businessName);
            UpdatedAt = null;
        }

        public void UpdateBuyerInformation(BuyerType buyerType, string? businessName)
        {
            if (!System.Enum.IsDefined(buyerType)) throw new ArgumentException("Invalid buyer type.");
            if (buyerType == BuyerType.Business && string.IsNullOrWhiteSpace(businessName))
                throw new ArgumentException("Business name is required for business buyers.");
            BuyerType = buyerType;
            BusinessName = buyerType == BuyerType.Business ? businessName!.Trim() : null;
            MarkAsUpdated();
        }
    }
}
