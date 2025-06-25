using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Responses.Tickets
{
    public class RefundNotification
    {
        public Guid TicketId { get; set; }
        public string EventTitle { get; set; }
        public DateTime RefundDate { get; set; }
    }
}
