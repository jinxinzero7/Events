using EventPlatform.Application.DTO.Requests.Events;
using EventPlatform.Application.DTO.Responses.Events;
using EventPlatform.Application.Interfaces;
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
        INotificationService _notificationService;
        IUserRepository _userRepository;
        IEventRepository _eventRepository;
        public EventService(IUserRepository userRepository, IEventRepository eventRepository, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _notificationService = notificationService;
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
            var eventTypeExists = _eventRepository.EventTypeExists(request.EventType);
            if (!eventTypeExists)
                throw new ArgumentException("Invalid event type.");

            // проверка даты
            if (request.EventTime < DateTime.UtcNow.AddHours(24))
                throw new ArgumentException("Event time must be at least 24 hours from now.");

            var newEvent = new Event
            {
                Status = EventStatusType.Moderated,
                OrganizerId = request.OrganizerId,
                EventType = request.EventType,
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
            return new EventResponse
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
        }

        public async Task<EventResponse> UpdateEventStatusAsync(Guid eventId, UpdateEventStatusRequest request)
        {
            var @event = await _eventRepository.GetEventByIdAsync(eventId);
            if (@event == null)
            {
                throw new ArgumentException("Event not found.");
            }

            @event.Status = request.Status;

            if (request.Status == EventStatusType.Rejected && !string.IsNullOrEmpty(request.Comment))
            {
                // отправка уведомления организатору
                var Organizer = await _userRepository.GetUserByIdAsync(@event.OrganizerId);
                await _notificationService.SendEmailNotification(
                    Organizer.Email,
                    @event.Title,
                    request.Comment
                );
            }

            await _eventRepository.UpdateEventAsync(@event);

            return new EventResponse
            {
                Id = @event.Id,
                OrganizerId = @event.OrganizerId,
                Title = @event.Title,
                Description = @event.Description,
                Address = @event.Address,
                EventTime = @event.EventTime,
                Status = @event.Status.ToString(),
                TicketPrice = @event.TicketPrice,
                AvailableTickets = @event.AvailableTickets,
                TicketQuantity = @event.TicketQuantity,
                EventType = @event.EventType,
            };
        }

        public async Task<IEnumerable<EventResponse>> GetOrganizerEventsAsync(Guid organizerId)
        {
            var events = await _eventRepository.GetEventsByOrganizerIdAsync(organizerId);
            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                OrganizerId = e.OrganizerId,
                Title = e.Title,
                Description = e.Description,
                Address = e.Address,
                EventTime = e.EventTime,
                Status = e.Status.ToString(),
                TicketPrice = e.TicketPrice,
                AvailableTickets = e.AvailableTickets,
                TicketQuantity = e.TicketQuantity,
                EventType = e.EventType,
            }).ToList();
        }

        public async Task<IEnumerable<EventResponse>> GetPendingModerationEventsAsync()
        {
            var events = await _eventRepository.GetPendingModerationEventsAsync();
            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                OrganizerId = e.OrganizerId,
                Title = e.Title,
                Description = e.Description,
                Address = e.Address,
                EventTime = e.EventTime,
                Status = e.Status.ToString(),
                TicketPrice = e.TicketPrice,
                AvailableTickets = e.AvailableTickets,
                TicketQuantity = e.TicketQuantity,
                EventType = e.EventType,
            }).ToList();
        }

        public async Task<List<EventResponse>> SearchEventsAsync(DateTime? dateFrom, DateTime? dateTo, List<Guid> tagIds, List<Guid> moodIds, EventType? eventType)
        {
            var events = await _eventRepository.SearchEventsAsync(dateFrom, dateTo, tagIds, moodIds, eventType);

            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                OrganizerId = e.OrganizerId,
                Title = e.Title,
                Description = e.Description,
                Address = e.Address,
                EventTime = e.EventTime,
                Status = e.Status.ToString(),
                TicketPrice = e.TicketPrice,
                AvailableTickets = e.AvailableTickets,
                TicketQuantity = e.TicketQuantity,
                EventType = e.EventType
            }).ToList();
        }


        public async Task<EventResponse> UpdateEventAsync(Guid eventId, Guid organizerId, EventUpdateRequest request)
        {
            var @event = await _eventRepository.GetEventByIdAsync(eventId);
            if (@event == null)
            {
                throw new ArgumentException("Event not found");
            }

            if (@event.OrganizerId != organizerId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this event.");
            }

            @event.Title = request.Title;
            @event.Description = request.Description;
            @event.Address = request.Address;
            @event.EventTime = request.EventTime;
            @event.TicketPrice = request.TicketPrice;
            @event.TicketQuantity = request.TicketQuantity;
            @event.AvailableTickets = request.TicketQuantity;
            @event.EventType = request.EventType;

            await _eventRepository.UpdateEventAsync(@event);

            return new EventResponse
            {
                Id = @event.Id,
                OrganizerId = @event.OrganizerId,
                Title = @event.Title,
                Description = @event.Description,
                Address = @event.Address,
                EventTime = @event.EventTime,
                Status = @event.Status.ToString(),
                TicketPrice = @event.TicketPrice,
                AvailableTickets = @event.AvailableTickets,
                TicketQuantity = @event.TicketQuantity,
                EventType = @event.EventType,
            };
        }

    }
}
