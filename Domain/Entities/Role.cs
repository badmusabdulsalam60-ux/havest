using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        private Role() { }
        public Role(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Role name is required.");
            Name = name.Trim();
            Description = description?.Trim();
        }
    }
}
