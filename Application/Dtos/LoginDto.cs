using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public  class LoginRequestModel
    {
        public string Email { get; set; } = default!;
        public string pass { get; set; } = default!;

    }

    public class LoginResponseModel
    {
        public string Token { get; set; } = default!;
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
