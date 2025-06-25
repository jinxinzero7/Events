using EventPlatform.Application.DTO.Responses.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateToken(UserResponse user);
    }
}
