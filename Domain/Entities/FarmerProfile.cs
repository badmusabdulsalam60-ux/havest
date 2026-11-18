using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class FarmerProfile : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public string FarmName { get; private set; } = string.Empty;
        public string FarmAddress { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public bool IsVerified { get; private set; }

        private FarmerProfile() { }

        public FarmerProfile(Guid userId, string farmName, string farmAddress, string state)
        {
            if (userId == Guid.Empty) throw new ArgumentException("User is required.");
            UserId = userId;
            UpdateFarmInformation(farmName, farmAddress, state);
            IsVerified = false;
            UpdatedAt = null;
        }

        public void UpdateFarmInformation(string farmName, string farmAddress, string state)
        {
            if (string.IsNullOrWhiteSpace(farmName) || string.IsNullOrWhiteSpace(farmAddress) ||
                string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("Farm name, address and state are required.");
            FarmName = farmName.Trim();
            FarmAddress = farmAddress.Trim();
            State = state.Trim();
            MarkAsUpdated();
        }
    }
}
