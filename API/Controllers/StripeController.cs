using Application.Orders;
using Application.Stripe;
using Application.Stripe.Charge;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StripeController : BaseApiController
    {
        [HttpPost("CreateCardCustomer")]
        public async Task<IActionResult> SubmitCardDetails(
            Application.Stripe.Card.CreateCardCustomerParam createCustomerParam
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Stripe.Card.CreateCardCustomer.Command
                    {
                        CreateCardCustomerParam = createCustomerParam
                    }
                )
            );
        }

        [HttpPost("CreateBankCustomer")]
        public async Task<IActionResult> SubmitBankDetails(
            Application.Stripe.Bank.CreateBankCustomerParam createBankParam
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Stripe.Bank.CreateBankCustomer.Command
                    {
                        CreateBankCustomerParam = createBankParam
                    }
                )
            );
        }

        [HttpGet("GetCustomerCharges")]
        public async Task<IActionResult> ChargeCustomer(Guid customerId)
        {
            return HandleResult(
                await Mediator.Send(new GetPaymentDetails.Query { CustomerId = customerId, })
            );
        }

        [HttpPost("Pay")]
        public async Task<IActionResult> Pay(
            CreateChargeParamDto chargeParamDto,
            Guid customerId,
            string method = "card"
        )
        {
            var cart = await Mediator.Send(new Cart.Query { CustomerId = customerId, Cart = true });

            if (!cart.IsSuccess)
                return HandleResult(cart);

            var amount = await Mediator.Send(new CalculateTotals.Query { OrderDto = cart.Value });

            if (!amount.IsSuccess)
                return HandleResult(amount);

            if (amount.Value.First().AmountDue != chargeParamDto.Amount)
                return Problem(
                    string.Format(
                        "{0} and {1} are not equaul",
                        chargeParamDto.Amount,
                        amount.Value.First().AmountDue
                    )
                );

            var payment = await Mediator.Send(
                new ChargeCustomer.Command
                {
                    CreateChargeParamDto = chargeParamDto,
                    CustomerId = customerId,
                    Method = method
                }
            );

            if (!payment.IsSuccess)
                return HandleResult(payment);

            var processPurchase = await Mediator.Send(
                new AfterPaymentProcess.Command { CustomerId = customerId }
            );

            if (!processPurchase.IsSuccess)
                return HandleResult(processPurchase);

            return HandleResult(payment);
        }
    }
}
