using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<Event> GetEventByIdAsync(Guid id);
        Task<List<Event>> GetEventsByOrganizerIdAsync(Guid organizerId);
        //Task<bool> EventTypeExists(Guid eventTypeId);
        Task<Event> CreateEventAsync(Event @event);

    }
}
