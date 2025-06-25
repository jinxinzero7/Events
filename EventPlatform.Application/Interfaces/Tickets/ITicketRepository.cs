using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces.Tickets
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateTicketAsync(Ticket @ticket);
        Task<Ticket> GetTicketByIdAsync(Guid id);
        Task UpdateTicketStatusAsync(Guid ticketId, TicketStatus status);
        Task<Ticket> GetByQrCodeAsync(string qrCode);
        Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId);
        Task<IEnumerable<Ticket>> GetEventTicketsAsync(Guid eventId);
    }
}
