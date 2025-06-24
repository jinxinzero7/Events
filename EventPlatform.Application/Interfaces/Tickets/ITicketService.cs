using EventPlatform.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces.Tickets
{
    public interface ITicketService
    {
        Task<TicketResponse> PurchaseTicketAsync(Guid EventId, Guid UserId);
    }
}
