using EventPlatform.Application.DTO.Requests.Users;
using EventPlatform.Application.DTO.Responses.Users;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse> LoginAsync(LoginRequest request);
        Task<UserProfileResponse> GetUserProfileAsync(Guid userId);
        Task<UserProfileResponse> UpdateUserProfileAsync(Guid userId, UpdateProfileRequest request);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
        Task<IEnumerable<AllUsersResponse>> GetAllUsersAsync(); // Для админа
        Task BlockUserAsync(Guid userId);
        Task UnblockUserAsync(Guid userId);
        Task<OrganizerPublicProfile> GetOrganizerPublicProfileAsync(Guid organizerId);
    }
}
