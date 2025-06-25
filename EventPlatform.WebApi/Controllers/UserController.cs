using EventPlatform.Application.DTO;
using EventPlatform.Application.DTO.Requests.Users;
using EventPlatform.Application.Interfaces.Tickets;
using EventPlatform.Application.Interfaces.Users;
using EventPlatform.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public UserController(IUserService userService, ITicketService ticketService)
        {
            _userService = userService;
            _ticketService = ticketService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var profile = await _userService.GetUserProfileAsync(userId);
                return Ok(profile);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var result = await _userService.UpdateUserProfileAsync(userId, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                await _userService.ChangePasswordAsync(userId, request);
                return Ok("Password changed successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> GetUserTickets()
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var tickets = await _ticketService.GetUserTicketsAsync(userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("tickets/{ticketId}/qrcode")]
        public async Task<IActionResult> GetTicketQrCode(Guid ticketId)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var qrCodeData = await _ticketService.GetTicketQrCodeAsync(userId, ticketId);
                return Ok(new { QrCodeData = qrCodeData });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
