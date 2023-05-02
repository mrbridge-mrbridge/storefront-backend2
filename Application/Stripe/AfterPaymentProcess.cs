using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stripe
{
    public class AfterPaymentProcess
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid CustomerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDataContext _context;

            public Handler(AppDataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var order = _context.Orders
                    .Include(o => o.Purchases)
                    .FirstOrDefault(
                        o =>
                            o.CustomerId == request.CustomerId
                            && o.OrderState == Domain.OrderStates.processing
                    );

                if (order == null || order.Purchases == null)
                    return Result<Unit>.Failure("Customer has no cart");
                try
                {
                    foreach (Domain.Purchase purchase in order.Purchases)
                    {
                        var product = _context.Products.Find(purchase.ProductId);
                        product.Quantity -= purchase.QuantityPurchased;
                        _context.Products.Update(product);
                    }

                    order.OrderState = Domain.OrderStates.shipping;
                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!success)
                        return Result<Unit>.Failure("Failed to process order");

                    return Result<Unit>.Success(new Unit());
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }
    }
}
