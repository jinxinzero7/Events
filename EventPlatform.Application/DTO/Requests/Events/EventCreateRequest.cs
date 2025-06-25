using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Requests.Events
{
    public class EventCreateRequest
    {
        public Guid OrganizerId { get; set; }
        public EventType EventType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime EventTime { get; set; }
        public decimal TicketPrice { get; set; }
        public int TicketQuantity { get; set; }
    }
}
