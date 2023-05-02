using Application.Core;
using MediatR;
using Persistence;

namespace Application.DiscountAndPromotions
{
    public class RemoveFromProduct
    {
        public class Command : IRequest<Result<string>>
        {
            public Guid DiscountId { get; set; }
            public List<Guid> ProductIds { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly AppDataContext _context;

            public Handler(AppDataContext context)
            {
                _context = context;
            }

            public async Task<Result<string>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var discount = _context.Discounts.Find(request.DiscountId);

                if (discount == null)
                {
                    return Result<string>.Failure("Discount does not exist");
                }

                foreach (Guid productId in request.ProductIds)
                {
                    var product = _context.Products
                        .Where(p => p.StoreId == discount.StoreId)
                        .First(p => p.ProductId == productId);

                    if (product != null)
                    {
                        product.DiscountId = Guid.Empty;
                        _context.Products.Update(product);
                    }
                }

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                {
                    return Result<string>.Failure("Failed to apply discount to product");
                }

                return Result<string>.Success("Discount successfully removed from product");
            }
        }
    }
}
