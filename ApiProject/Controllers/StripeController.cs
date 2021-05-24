using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [Route("api/Stripe")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Processing(string stripeToken, string stripeEmail)
        {
            var options = new ChargeCreateOptions
            {
                Amount = 100,
                Currency = "PKR",
                Description = "Payment for intellectual property",
                Source = stripeToken,
                ReceiptEmail = stripeEmail
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
            return Ok();
        }
    }
}
