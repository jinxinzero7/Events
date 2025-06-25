using EventPlatform.Application.DTO.Requests.Events;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Application.Interfaces.Tickets;
using EventPlatform.Domain.Models;
using EventPlatform.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;

        public EventController(IEventService eventService, ITicketService ticketService)
        {
            _eventService = eventService;
            _ticketService = ticketService;
        }

        [HttpPost]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateRequest request)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                request.OrganizerId = userId;
                var result = await _eventService.CreateEventAsync(request);
                return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            try
            {
                var eventResponse = await _eventService.GetEventByIdAsync(id);
                return Ok(eventResponse);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchEvents(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] List<Guid> tagIds,
            [FromQuery] List<Guid> moodIds,
            [FromQuery] EventType? eventType)
        {
            try
            {
                var events = await _eventService.SearchEventsAsync(dateFrom, dateTo, tagIds, moodIds, eventType);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventUpdateRequest request)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var result = await _eventService.UpdateEventAsync(id, userId, request);
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
        }

        [HttpPost("{eventId}/purchase")]
        [Authorize]
        public async Task<IActionResult> PurchaseTicket(Guid eventId)
        {
            try
            {
                var userId = User.GetCurrentUserId();
                var result = await _ticketService.PurchaseTicketAsync(eventId, userId);
                return Ok(result);
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
    }
}
