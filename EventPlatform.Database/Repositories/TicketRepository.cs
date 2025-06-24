using EventPlatform.Application.Interfaces.Tickets;
using EventPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Database.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TicketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Ticket> CreateTicketAsync(Ticket @ticket)
        {
            _dbContext.Tickets.Add(@ticket);
            await _dbContext.SaveChangesAsync();
            return @ticket;
        }
        public async Task<Ticket> GetTicketByIdAsync(Guid id)
        {
            return await _dbContext.Tickets.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task UpdateTicketStatusAsync(Guid ticketId, TicketStatus status)
        {
            var @ticket = await _dbContext.Tickets.FindAsync(ticketId);
            if (@ticket != null) 
            {
                @ticket.Status = status;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
