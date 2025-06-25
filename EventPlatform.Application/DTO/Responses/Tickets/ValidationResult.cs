using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Responses.Tickets
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string EventName { get; set; }
        public string UserName { get; set; }
        public TicketStatus Status { get; set; }
    }
}
