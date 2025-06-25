using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Responses.Tickets
{
    public class RefundResult
    {
        public bool Success { get; set; }
        public TicketStatus NewStatus { get; set; }
    }
}
