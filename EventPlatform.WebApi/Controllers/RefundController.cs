using EventPlatform.Application.DTO.Requests.Tickets;
using EventPlatform.Application.Interfaces.Tickets;
using EventPlatform.Domain.Models;
using EventPlatform.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("api/refunds")]
    [Authorize]
    public class RefundController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public RefundController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestRefund([FromBody] RefundRequest request)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var result = await _ticketService.RequestRefundAsync(request.TicketId, userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("status/{ticketId}")]
        public async Task<IActionResult> GetRefundStatus(Guid ticketId)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var status = await _ticketService.GetRefundStatusAsync(ticketId, userId);
                return Ok(new { Status = status });
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
