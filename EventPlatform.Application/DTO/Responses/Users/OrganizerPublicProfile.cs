using EventPlatform.Application.DTO.Responses.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Responses.Users
{
    public class OrganizerPublicProfile
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Description { get; set; }
        public IEnumerable<EventShortResponse> Events { get; set; }
    }
}
