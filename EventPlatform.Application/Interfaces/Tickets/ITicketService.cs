using EventPlatform.Application.DTO.Responses.Tickets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces.Tickets
{
    public interface ITicketService
    {
        Task<TicketResponse> PurchaseTicketAsync(Guid EventId, Guid UserId);
        Task<IEnumerable<TicketResponse>> GetUserTicketsAsync(Guid userId);
        Task<string> GetTicketQrCodeAsync(Guid userId, Guid ticketId);
        Task<DTO.Responses.Tickets.ValidationResult> ValidateTicketAsync(string qrCodeData);
        Task<RefundResult> RequestRefundAsync(Guid ticketId, Guid userId);
        Task<RefundStatus> GetRefundStatusAsync(Guid ticketId, Guid userId);
        Task<IEnumerable<RefundNotification>> GetRefundNotificationsAsync(Guid organizerId);
    }
}
