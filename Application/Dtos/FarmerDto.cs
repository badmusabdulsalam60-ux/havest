using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public  class FarmerDto
    {
        public Guid UserId { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public string FarmAddress { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public bool IsVerified { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

    }

    public class CreateFarmerProfileRequestModel
    {
        public string FarmName { get;  set; } = string.Empty;
        public string FarmAddress { get;  set; } = string.Empty;
        public string State { get;  set; } = string.Empty;
        public bool IsVerified { get;  set; }
        public string Password { get; set; } = string.Empty; 
        public string FirstName { get;  set; } = string.Empty;
        public string LastName { get;  set; } = string.Empty;
        public string UserName { get;  set; } = string.Empty;
        public string Email { get;  set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class CreateFarmerProfileResponseModel
    {

        public Guid Id { get; set; }
    }

    public class UpdateFarmerProfileRequestModel
    {
        public string FarmName { get; set; } = string.Empty;
        public string FarmAddress { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
