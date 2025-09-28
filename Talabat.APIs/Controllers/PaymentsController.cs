using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.DTOs;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string webhookSecret = "";

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }


        [HttpPost("{basketId}")] 
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePayment(string basketId)
        {
            CustomerBasket? basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null)
            {
                return BadRequest(new ApiResponse(400, "A Problem with your basket"));
            }
            else
            {
                return Ok(basket);
            }
        }



        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            Event stripeEvent = EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"],
                webhookSecret,throwOnApiVersionMismatch: false);

            PaymentIntent? paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            Order order;
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    order = await _paymentService.UpdatePaymentIntentIdSucceededOrFailed(paymentIntent.Id, true);
                    _logger.LogInformation("Payment is Succeeded", paymentIntent.Id);
                    break;
                case "payment_intent.payment_failed":
                    order = await _paymentService.UpdatePaymentIntentIdSucceededOrFailed(paymentIntent.Id, false);
                    _logger.LogInformation("Payment is Failed :(", paymentIntent.Id);
                    break;
                default:
                    _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }
            return Ok();
        }

    }
}
