using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPlatform.Application.DTO.Responses.Events;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlatform.Database.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EventRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Event> GetEventByIdAsync(Guid id)
        {
            return await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(Guid organizerId)
        {
            return await _dbContext.Events
                .Where(e => e.OrganizerId == organizerId)
                .ToListAsync();
        }

        public bool EventTypeExists(EventType eventType)
        {
            return Enum.IsDefined(typeof(EventType), eventType);
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            _dbContext.Events.Add(@event);
            await _dbContext.SaveChangesAsync();
            return @event;
        }

        public async Task DecrementAvailableTickets(Guid eventId, int count)
        {
            var @event = await _dbContext.Events.FindAsync(eventId);
            if (@event != null)
            {
                @event.AvailableTickets -= count;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateEventAsync(Event @event)
        {
            _dbContext.Events.Update(@event);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetPendingModerationEventsAsync()
        {
            return await _dbContext.Events
                .Where(e => e.Status == EventStatusType.Moderated)
                .ToListAsync();
        }

        public async Task<List<Event>> SearchEventsAsync(DateTime? dateFrom, DateTime? dateTo, List<Guid> tagIds, List<Guid> moodIds, EventType? eventType)
        {
            var query = _dbContext.Events.AsQueryable();

            if (dateFrom.HasValue) query = query.Where(e => e.EventTime >= dateFrom.Value);
            if (dateTo.HasValue) query = query.Where(e => e.EventTime <= dateTo.Value);

            // фильтрация по тегам
            if (tagIds != null && tagIds.Any())
            {
                query = query.Where(e => _dbContext.EventTags.Any(et => tagIds.Contains(et.TagId) && et.EventId == e.Id));
            }

            // фильтрация по настроению
            if (moodIds != null && moodIds.Any())
            {
                query = query.Where(e => _dbContext.EventMoods.Any(em => moodIds.Contains(em.MoodId) && em.EventId == e.Id));
            }

            // фильтрация по типу мероприятия
            if (eventType.HasValue)
            {
                query = query.Where(e => e.EventType == eventType.Value);
            }

            var events = await query.ToListAsync();

            return events;
        }
    }
}


