using EventPlatform.Application.DTO.Requests.Tickets;
using EventPlatform.Application.DTO.Responses.Tickets;
using EventPlatform.Application.Interfaces;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Application.Interfaces.Tickets;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        IPaymentService _paymentService;

        // Unit of Work pattern
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IEventRepository eventRepository, ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
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

        public async Task<DTO.Responses.Tickets.ValidationResult> ValidateTicketAsync(string qrCodeData)
        {
            var ticket = await _ticketRepository.GetByQrCodeAsync(qrCodeData);
            if (ticket == null) return new DTO.Responses.Tickets.ValidationResult { IsValid = false };

            return new DTO.Responses.Tickets.ValidationResult
            {
                IsValid = ticket.Status == TicketStatus.Активный,
                EventName = ticket.Event.Title,
                UserName = ticket.User?.Username,
                Status = ticket.Status
            };
        }

        public async Task<RefundResult> RequestRefundAsync(Guid ticketId, Guid userId)
        {
            var ticket = await _ticketRepository.GetTicketByIdWithEventAsync(ticketId);
            if (ticket == null) throw new ArgumentException("Ticket not found");
            if (ticket.UserId != userId) throw new UnauthorizedAccessException();

            // Проверяем возможность возврата (не менее чем за 24 часа до мероприятия)
            if (ticket.Event.EventTime < DateTime.UtcNow.AddHours(24))
                throw new InvalidOperationException("Refund is not allowed less than 24 hours before the event");

            await _ticketRepository.UpdateTicketStatusAsync(ticket.Id, TicketStatus.Возвращен);

            // Возвращаем средства
            bool refundSuccess = await _paymentService.ProcessRefund(new RefundRequest
            {
                TicketId = ticket.Id,
            });

            return new RefundResult
            {
                Success = refundSuccess,
                NewStatus = ticket.Status
            };
        }

        public async Task<IEnumerable<TicketResponse>> GetUserTicketsAsync(Guid userId)
        {
            var tickets = await _ticketRepository.GetUserTicketsAsync(userId);
            return tickets.Select(ticket => new TicketResponse
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                EventTitle = ticket.Event.Title,
                QrCodeData = ticket.QrCodeData,
                PurchaseDate = ticket.PurchaseDate,
                Status = ticket.Status,
            }).ToList();
        }

        public async Task<string> GetTicketQrCodeAsync(Guid userId, Guid ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new ArgumentException("Ticket not found");
            }

            if (ticket.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this ticket.");
            }

            return ticket.QrCodeData;
        }

        public async Task<RefundStatus> GetRefundStatusAsync(Guid ticketId, Guid userId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new ArgumentException("Ticket not found");
            }

            if (ticket.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this ticket.");
            }

            return ticket.Status == TicketStatus.Возвращен ? RefundStatus.Approved : RefundStatus.Pending;
        }

        public async Task<IEnumerable<RefundNotification>> GetRefundNotificationsAsync(Guid organizerId)
        {
            var tickets = await _ticketRepository.GetEventTicketsAsync(organizerId);

            var refundNotifications = tickets
                .Where(t => t.Status == TicketStatus.Возвращен)
                .Select(ticket => new RefundNotification
                {
                    TicketId = ticket.Id,
                    EventTitle = ticket.Event.Title,
                    
                })
                .ToList();

            return refundNotifications;
        }

    }
}
