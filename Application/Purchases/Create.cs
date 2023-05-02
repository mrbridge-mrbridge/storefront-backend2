using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Purchases
{
	public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public PurchaseCreateParam PurchaseCreateParam { get; set; }
            public Guid OrderId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var productToOrder = _context.Products.Find(
                    request.PurchaseCreateParam.ProductId
                );

                if (productToOrder == null || productToOrder.Quantity < request.PurchaseCreateParam.QuantityPurchased)
                {
                    return Result<Unit>.Failure("Please we are out of stock");
                }

                try
                {
                    var orderToUpdate = _context.Orders
						.Include(o => o.Purchases)
						.First(o => o.OrderId == request.OrderId);

                    if (orderToUpdate == null)
                        return Result<Unit>.Failure("Cart does not exist");

                    var newpurchase = _mapper.Map<Domain.Purchase>(request.PurchaseCreateParam);
					newpurchase.Discount = 0;

					orderToUpdate.Purchases.Add(newpurchase);
					_context.Orders.Update(orderToUpdate);

                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;
		
                    if (success)
                        return Result<Unit>.Success(new Unit());

                    return Result<Unit>.Failure("Purchase failed");
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }
    }
}
