using EventPlatform.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces
{
    public interface IEventService
    {
        Task<EventResponse> CreateEventAsync(EventCreateRequest request, Guid organizerId);
        Task<EventResponse> GetEventByIdAsync(Guid id);
    }
}
