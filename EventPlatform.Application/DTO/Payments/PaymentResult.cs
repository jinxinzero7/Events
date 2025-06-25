using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.DTO.Payments
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
    }
}
