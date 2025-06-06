using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPlatform.Application.Interfaces;
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

        public async Task<List<Event>> GetEventsByOrganizerIdAsync(Guid organizerId)
        {
            return await _dbContext.Events.Where(e => e.OrganizerId == organizerId).ToListAsync();
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            _dbContext.Events.Add(@event);
            await _dbContext.SaveChangesAsync();
            return @event;
        }
    }
}
