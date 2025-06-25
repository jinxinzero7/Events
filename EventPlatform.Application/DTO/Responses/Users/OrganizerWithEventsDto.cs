using EventPlatform.Application.DTO.Responses.Events;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Responses.Users
{
    public class OrganizerWithEventsDto
    {
        public User User { get; set; }
        public List<Event> Events { get; set; }

        public OrganizerPublicProfile ToPublicProfile()
        {
            return new OrganizerPublicProfile
            {
                Id = User.Id,
                Username = User.Username,
                Description = User.Description,
                Events = Events.Select(e => new EventShortResponse
                {
                    Id = e.Id,
                    Title = e.Title,
                    EventTime = e.EventTime
                }).ToList()
            };
        }
    }
}
