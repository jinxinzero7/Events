using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Payments
{
    public class PaymentRequest
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
