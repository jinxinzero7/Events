using EventPlatform.Application.DTO.Requests.Events;
using EventPlatform.Application.DTO.Responses.Events;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces.Events
{
    public interface IEventService
    {
        Task<EventResponse> CreateEventAsync(EventCreateRequest request);
        Task<EventResponse> GetEventByIdAsync(Guid id);
        Task<IEnumerable<EventResponse>> GetOrganizerEventsAsync(Guid organizerId);
        Task<IEnumerable<EventResponse>> GetPendingModerationEventsAsync();
        Task<EventResponse> UpdateEventStatusAsync(Guid eventId, UpdateEventStatusRequest request);
        Task<List<EventResponse>> SearchEventsAsync(DateTime? dateFrom, DateTime? dateTo, List<Guid> tagIds, List<Guid> moodIds, EventType? eventType);

        // можно будет добавить позже
        // Task<IEnumerable<EventMapResponse>> GetEventsForMapAsync(DateTime? date, string[] tags);
        Task<EventResponse> UpdateEventAsync(Guid eventId, Guid organizerId, EventUpdateRequest request);
    }
}
