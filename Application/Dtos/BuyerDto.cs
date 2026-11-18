using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public  class BuyerDto
    {
        public Guid UserId { get; set; }
        public BuyerType BuyerType { get; set; }
        public string? BusinessName { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

    }


    public class CreateBuyerProfileRequestModel
    {
       
        public BuyerType BuyerType { get;  set; }
        public string? BusinessName { get;  set; }
        public string FirstName { get;  set; } = string.Empty;
        public string LastName { get;  set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public string UserName { get;  set; } = string.Empty;
        public string Email { get;  set; } = string.Empty;
        public string PhoneNumber { get;  set; } = string.Empty;
    }

    public class  CreateBuyerProfileResponseModel
    {
        public Guid Id { get; set; }
    }

    public class UpdateBuyerProfileRequestModel
    {
        public BuyerType BuyerType { get; set; }
        public string? BusinessName { get; set; }
    }
}
