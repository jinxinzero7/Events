using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO
{
    public class TicketResponse
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventTitle { get; set; }
        public string QrCodeData { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
