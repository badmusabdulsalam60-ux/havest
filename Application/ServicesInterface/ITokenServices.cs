using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServicesInterface
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
