using EventPlatform.Application.DTO.Requests.Tickets;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Application.Interfaces.Tickets;
using EventPlatform.Domain.Models;
using EventPlatform.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("api/organizer")]
    [Authorize(Roles = "Organizer")]
    public class OrganizerController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;

        public OrganizerController(IEventService eventService, ITicketService ticketService)
        {
            _eventService = eventService;
            _ticketService = ticketService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetOrganizerEvents()
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var events = await _eventService.GetOrganizerEventsAsync(userId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("tickets/validate")]
        public async Task<IActionResult> ValidateTicket([FromBody] ValidateTicketRequest request)
        {
            try
            {
                var result = await _ticketService.ValidateTicketAsync(request.QrCodeData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("refunds")]
        public async Task<IActionResult> GetRefundNotifications()
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var refunds = await _ticketService.GetRefundNotificationsAsync(userId);
                return Ok(refunds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
