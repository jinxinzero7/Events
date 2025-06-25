using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Responses.Events
{
    public class EventShortResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime EventTime { get; set; }
    }
}
