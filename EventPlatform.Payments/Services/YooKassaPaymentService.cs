using EventPlatform.Application.DTO.Payments;
using EventPlatform.Application.DTO.Requests.Tickets;
using EventPlatform.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Payments.Services
{
    public class YooKassaPaymentService : IPaymentService
    {
        public Task<PaymentResult> ProcessPayment(PaymentRequest request)
        {
            // Заглушка для MVP
            return Task.FromResult(new PaymentResult { Success = true });
        }

        public Task<bool> ProcessRefund(RefundRequest request)
        {
            // Заглушка для MVP
            return Task.FromResult(true);
        }
    }
}
