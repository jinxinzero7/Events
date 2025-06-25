using EventPlatform.Application.DTO.Payments;
using EventPlatform.Application.DTO.Requests.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPayment(PaymentRequest request);
        Task<bool> ProcessRefund(RefundRequest request);
    }
}
