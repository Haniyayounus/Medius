using Application.IRepository;
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
        private readonly IUserIp userIpService;

        public StripeController(IUserIp _userIpService)
        {
            this.userIpService = _userIpService;
        }

        [HttpPost]
        public async Task<IActionResult> Charge(Guid id, Guid userId, string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            try
            {
                var customer = customerService.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken
                });

                string customerID = Convert.ToString(userId);
                var charge = chargeService.Create(new ChargeCreateOptions
                {
                    Amount = 500,
                    Description = "Medius Project",
                    Currency = "usd",
                    Customer = customerID,
                    ReceiptEmail = stripeEmail,
                    //Metadata = new Dictionary<string, string>()
                    //{
                    //    { "OrderId" , "111", },
                    //    { "Postcode" , "LEE11" },
                    //}
                });

                if (charge.Status == "Succeed")
                {
                    string BalanceTransactionId = charge.BalanceTransactionId;
                    return StatusCode(StatusCodes.Status200OK, BalanceTransactionId);
                }
                else
                {
                    var data = await userIpService.DeleteIP(userId, id);
                    return StatusCode(StatusCodes.Status200OK, data);
                }
            }
            catch (Exception ex)
            {
                // Log exception code goes here
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
