using EventPlatform.Application.DTO;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterRequest request);
        Task<User> LoginAsync(LoginRequest request);
    }
}
