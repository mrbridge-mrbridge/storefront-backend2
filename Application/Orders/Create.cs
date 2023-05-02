using Application.Core;
using MediatR;
using Persistence;

namespace Application.Orders
{
    public class Create
    {
        public class Command : IRequest<Result<Guid>>
        {
            public OrderCreateParam OrderCreateParam { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Guid>>
        {
            private readonly AppDataContext _context;

            public Handler(AppDataContext context)
            {
                _context = context;
            }

            public async Task<Result<Guid>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var customer = _context.Customers.Find(request.OrderCreateParam.CustomerId);
                
                if (customer == null)
                    return Result<Guid>.Failure("Customer does not exist");

                var existingOrder = _context.Orders.FirstOrDefault(
                    o =>
                        o.CustomerId == request.OrderCreateParam.CustomerId
                        && o.OrderState == Domain.OrderStates.processing
                );

                if (existingOrder == null)
                {
                    var newOrder = new Domain.Order
                    {
                        CustomerId = request.OrderCreateParam.CustomerId,
                        OrderState = Domain.OrderStates.processing,
                    };

                    _context.Orders.Add(newOrder);

                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (success)
                    {
                        return Result<Guid>.Success(newOrder.OrderId);
                    }

                    return Result<Guid>.Failure("Placing order failed");
                }
                else
                {
                    return Result<Guid>.Success(existingOrder.OrderId);
                }
            }
        }
    }
}
