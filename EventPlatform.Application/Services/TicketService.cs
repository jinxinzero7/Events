using EventPlatform.Application.DTO;
using EventPlatform.Application.Interfaces;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Application.Interfaces.Tickets;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Services
{
    public class TicketService : ITicketService
    {
        IEventRepository _eventRepository;
        ITicketRepository _ticketRepository;

        // Unit of Work pattern
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IEventRepository eventRepository, ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _unitOfWork = unitOfWork;
        }

        public async Task<TicketResponse> PurchaseTicketAsync(Guid EventId, Guid UserId)
        {
            var @event = await _eventRepository.GetEventByIdAsync(EventId);

            //валидация на существование мероприятия по его Id
            if (@event == null) 
            {
                throw new ArgumentException("Event not found");
            }

            // валидация на доступные билеты
            if (@event.AvailableTickets < 1)
                throw new InvalidOperationException("No tickets available");

            //валидация на время события
            if (@event.EventTime < DateTime.UtcNow)
                throw new InvalidOperationException("Event has already started");

            var @ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                EventId = EventId,
                UserId = UserId,
                PurchaseDate = DateTime.UtcNow,
                Status = TicketStatus.Активный,
                QrCodeData = GenerateSecureQrCode(EventId, UserId)
            };

            // транзакция
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _ticketRepository.CreateTicketAsync(ticket);
                await _eventRepository.DecrementAvailableTickets(EventId, 1);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                // Откатываем при ошибке
                await _unitOfWork.RollbackAsync();
                throw new Exception("Purchase failed", ex);
            }

            await _ticketRepository.CreateTicketAsync(@ticket);
            await _eventRepository.DecrementAvailableTickets(EventId, 1);

            return new TicketResponse
            {
                Id = @ticket.Id,
                EventId = @ticket.EventId,
                EventTitle = @event.Title,
                QrCodeData = @ticket.QrCodeData,
                PurchaseDate = @ticket.PurchaseDate
            };
        }

        private string GenerateSecureQrCode(Guid eventId, Guid userId)
        {
            // соль
            const string pepper = "EVENT_SECRET_2025";
            var timestamp = DateTime.UtcNow.ToString("O");

            var rawData = $"{eventId}|{userId}|{timestamp}|{pepper}";

            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            return Convert.ToBase64String(hash);
        }
    }
}
