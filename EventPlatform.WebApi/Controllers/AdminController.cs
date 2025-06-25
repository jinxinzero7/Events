using EventPlatform.Application.DTO.Requests.Events;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Application.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public AdminController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        [HttpGet("events/pending")]
        public async Task<IActionResult> GetPendingEvents()
        {
            try
            {
                var events = await _eventService.GetPendingModerationEventsAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("events/{id}/status")]
        public async Task<IActionResult> UpdateEventStatus(
            Guid id,
            [FromBody] UpdateEventStatusRequest request)
        {
            try
            {
                var result = await _eventService.UpdateEventStatusAsync(id, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("users/{id}/block")]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            try
            {
                await _userService.BlockUserAsync(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("users/{id}/unblock")]
        public async Task<IActionResult> UnblockUser(Guid id)
        {
            try
            {
                await _userService.UnblockUserAsync(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
