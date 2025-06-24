using System.Security;
using System.Security.Claims;

namespace EventPlatform.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetCurrentUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new SecurityException("Invalid user ID");
            }

            return userId;
        }
    }
}
