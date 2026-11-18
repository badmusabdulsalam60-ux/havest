using Domain.Shared;
using System;

namespace Domain.Entities
{
    public class Crop : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; } = true;

        private Crop() { }

        public Crop(string name, string category, string? description = null)
        {
            Update(name, category, description);
            UpdatedAt = null;
        }

        public void Update(string name, string category, string? description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Crop name is required.");
            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Crop category is required.");
            Name = name.Trim();
            Category = category.Trim();
            Description = description?.Trim();
            MarkAsUpdated();
        }
    }
}
