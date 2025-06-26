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

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            _dbContext.Tickets.Add(ticket);
            await _dbContext.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> GetTicketByIdAsync(Guid ticketId)
        {
            return await _dbContext.Tickets.FirstOrDefaultAsync(e => e.Id == ticketId).ConfigureAwait(false);
        }

        public async Task<Ticket> GetTicketByIdWithEventAsync(Guid ticketId)
        {
            return await _dbContext.Tickets
                .Include(t => t.Event)  // добавляем загрузку события
                .FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task UpdateTicketStatusAsync(Guid ticketId, TicketStatus status)
        {
            var ticket = await _dbContext.Tickets.FindAsync(ticketId).ConfigureAwait(false);
            if (ticket != null)
            {
                ticket.Status = status;
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<Ticket> GetByQrCodeAsync(string qrCode)
        {
            return await _dbContext.Tickets
                .Include(t => t.Event)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.QrCodeData == qrCode)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId)
        {
            return await GetTicketsWithEvent()
                .Where(t => t.UserId == userId)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Ticket>> GetEventTicketsAsync(Guid organizerId)
        {
            return await _dbContext.Tickets
                .Where(t => t.Event.OrganizerId == organizerId)
                .Include(t => t.User)
                .Include(t => t.Event)
                .ToListAsync().ConfigureAwait(false);
        }

        private IQueryable<Ticket> GetTicketsWithEvent()
        {
            return _dbContext.Tickets.Include(t => t.Event);
        }
    }
}
