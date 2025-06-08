using EventPlatform.Application.DTO;
using EventPlatform.Application.Interfaces;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Services
{
    public class EventService : IEventService
    {
        IUserRepository _userRepository;
        IEventRepository _eventRepository;
        public EventService(IUserRepository userRepository, IEventRepository eventRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task<EventResponse> CreateEventAsync(EventCreateRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                OrganizerId = request.OrganizerId,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                EventTime = request.EventTime,
                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.CreateEventAsync(newEvent);
            var eventResponse = new EventResponse
            {
                Id = newEvent.Id,
                OrganizerId = newEvent.OrganizerId,
                Title = newEvent.Title,
                Description = newEvent.Description,
                Address = newEvent.Address,
                EventTime = newEvent.EventTime,
                CreatedAt = newEvent.CreatedAt
            };

            return eventResponse;
        }

        public async Task<EventResponse> GetEventByIdAsync(Guid id)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null) 
            {
                return null;
            }
            var responseEvent = new EventResponse
            {
                OrganizerId = existingEvent.OrganizerId,
                Title = existingEvent.Title,
                Description = existingEvent.Description,
                Address = existingEvent.Address,
                EventTime = existingEvent.EventTime,
                CreatedAt = existingEvent.CreatedAt
            };
            return responseEvent;
        }

    }
}
