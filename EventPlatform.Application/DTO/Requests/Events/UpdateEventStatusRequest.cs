using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Requests.Events
{
    public class UpdateEventStatusRequest
    {
        public EventStatusType Status { get; set; }
        public string Comment { get; set; }
    }
}
