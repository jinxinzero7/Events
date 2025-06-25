using EventPlatform.Application.DTO.Responses.Events;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces.Events
{
    public interface IEventRepository
    {
        Task<Event> GetEventByIdAsync(Guid id);
        Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(Guid organizerId);
        bool EventTypeExists(EventType eventType);
        Task<Event> CreateEventAsync(Event @event);
        Task DecrementAvailableTickets(Guid eventId, int count);
        Task<IEnumerable<Event>> GetPendingModerationEventsAsync();
        Task UpdateEventAsync(Event @event);
        Task<List<Event>> SearchEventsAsync(DateTime? dateFrom, DateTime? dateTo, List<Guid> tagIds, List<Guid> moodIds, EventType? eventType);

    }
}
