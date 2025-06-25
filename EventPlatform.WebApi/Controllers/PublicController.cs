using EventPlatform.Application.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : ControllerBase
    {
        private readonly IUserService _userService;

        public PublicController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("organizers/{id}")]
        public async Task<IActionResult> GetOrganizerProfile(Guid id)
        {
            try
            {
                var organizer = await _userService.GetOrganizerPublicProfileAsync(id);
                if (organizer == null) return NotFound();
                return Ok(organizer);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
