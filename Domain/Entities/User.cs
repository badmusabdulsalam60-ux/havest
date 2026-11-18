using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string UserName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public Guid RoleId { get; private set; }
        public Role Role { get; private set; } = null!;
        public bool IsActive { get; private set; } = true;
        public string FullName => $"{FirstName} {LastName}";

        private User() { }

        public User(string firstName, string lastName, string userName, string email,
            string password, string phoneNumber, Guid roleId)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("All user fields are required.");

            if (roleId == Guid.Empty) throw new ArgumentException("Role is required.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            UserName = userName.Trim();
            Email = email.Trim().ToLowerInvariant();
            PasswordHash =  BCrypt.Net.BCrypt.HashPassword(password);
            PhoneNumber = phoneNumber.Trim();
            RoleId = roleId;
        }

        public void UpdateContactDetails(string firstName, string lastName, string userName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Contact details cannot be empty.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            UserName = userName.Trim();
            PhoneNumber = phoneNumber.Trim();
            MarkAsUpdated();
        }

        public void ChangePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash is required.");
            PasswordHash = passwordHash;
            MarkAsUpdated();
        }

        public void Deactivate()
        {
            IsActive = false;
            MarkAsUpdated();
        }
    }
}
