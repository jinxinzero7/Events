using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateTicketAsync(Ticket ticket);
        Task<Ticket> GetTicketByIdAsync(Guid id);
        Task UpdateTicketStatusAsync(Guid ticketId, TicketStatus status);
    }
}
