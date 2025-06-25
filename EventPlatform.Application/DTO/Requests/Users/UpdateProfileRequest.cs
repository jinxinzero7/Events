using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Requests.Users
{
    public class UpdateProfileRequest
    {
        public string? Username { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
    }
}
