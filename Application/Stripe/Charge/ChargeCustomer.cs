using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stripe.Charge
{
    public class ChargeCustomer
    {
        public class Command : IRequest<Result<ChargeResource>>
        {
            public CreateChargeParamDto CreateChargeParamDto { get; set; }
            public Guid CustomerId { get; set; }
            public string Method { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<ChargeResource>>
        {
            private readonly IStripeService _stripeService;
            private readonly AppDataContext _context;

            public Handler(IStripeService stripeService, AppDataContext context)
            {
                _stripeService = stripeService;
                _context = context;
            }

            public async Task<Result<ChargeResource>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var order = _context.Orders
                    .Include(c => c.Charge)
                    .First(o => o.CustomerId == request.CustomerId);

                if (order == null)
                    return Result<ChargeResource>.Failure("order Does not exist");

                string stripeId;

                var customer = _context.Customers
                    .Include(c => c.CreditCardDetails)
                    .First(c => c.Id == request.CustomerId);

                if (request.Method != "card")
                    stripeId = customer.CreditCardDetails
                        .Where(c => c.Method == "bank")
                        .Single()
                        .StripeId;
                else
                    stripeId = customer.CreditCardDetails
                        .Where(c => c.Method == "card")
                        .Single()
                        .StripeId;

                var createChargeParam = new CreateChargeParam()
                {
                    CustomerStripeId = stripeId,
                    ReceiptEmail = "test@test.com",
                    Currency = "EUR",
                    Description = request.CreateChargeParamDto.Description,
                    Amount = request.CreateChargeParamDto.Amount
                };

                try
                {
                    var chargeResource = await _stripeService.CreateCharge(
                        createChargeParam,
                        cancellationToken
                    );

                    var newcharge = new Domain.Charge
                    {
                        ChargeId = chargeResource.ChargeId,
                        OrderId = order.OrderId
                    };

                    order.Charge = newcharge;

                    _context.Orders.Update(order);

                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!success)
                        return Result<ChargeResource>.Failure("charge update failed");

                    return Result<ChargeResource>.Success(chargeResource);
                }
                catch (Exception ex)
                {
                    return Result<ChargeResource>.Failure(ex.Message);
                }
            }
        }
    }
}
