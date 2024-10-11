using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return Ok(paymentRepository.All().ToList());
        }

        // GET: api/Payments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(Guid id)
        {
            var payment = paymentRepository.GeT(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}/total/{totalAmount}")]
        //[Authorize]
        public async Task<IActionResult> PutPayment(Guid id, decimal totalAmount)
        {
            var payment = paymentRepository.UpdateAsync(id, totalAmount);

            if (payment is null) 
                    return NotFound();
            paymentRepository.SaveChanges();
            return NoContent();
        }

       
    }
}
