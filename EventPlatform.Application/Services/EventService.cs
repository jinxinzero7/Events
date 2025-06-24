using EventPlatform.Application.DTO;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Application.Interfaces.Users;
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
            //проверка на null
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            //проверка роли
            var organizer = await _userRepository.GetUserByIdAsync(request.OrganizerId);
            if (organizer?.AccountType != AccountType.Organizer)
                throw new UnauthorizedAccessException("Only organizers can create events.");

            //проверка типа мероприятия
            var eventTypeExists = await _eventRepository.EventTypeExists(request.EventTypeId);
            if (!eventTypeExists)
                throw new ArgumentException("Invalid event type.");

            // проверка даты
            if (request.EventTime < DateTime.UtcNow.AddHours(24))
                throw new ArgumentException("Event time must be at least 24 hours from now.");

            var newEvent = new Event
            {
                Status = EventStatusType.Модерируется,
                OrganizerId = request.OrganizerId,
                EventTypeId = request.EventTypeId,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                EventTime = request.EventTime,
                TicketPrice = request.TicketPrice,
                TicketQuantity = request.TicketQuantity,
                AvailableTickets = request.TicketQuantity
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
                Status = newEvent.Status.ToString(),
                TicketPrice = newEvent.TicketPrice,
                AvailableTickets = newEvent.AvailableTickets,
                TicketQuantity = newEvent.TicketQuantity
            };

            return eventResponse;
        }

        public async Task<EventResponse> GetEventByIdAsync(Guid id)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null) 
            {
                throw new ArgumentNullException("Events with this id do not exist");
            }
            var responseEvent = new EventResponse
            {
                Id = existingEvent.Id,
                OrganizerId = existingEvent.OrganizerId,
                Title = existingEvent.Title,
                Description = existingEvent.Description,
                Address = existingEvent.Address,
                EventTime = existingEvent.EventTime,
                Status = existingEvent.Status.ToString(),
                TicketPrice = existingEvent.TicketPrice,
                AvailableTickets = existingEvent.AvailableTickets
            };
            return responseEvent;
        }

    }
}
