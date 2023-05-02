using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.DiscountAndPromotions
{
    public class ApplyToProduct
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
                        product.DiscountId = discount.DiscountId;
                        _context.Products.Update(product);
                    }
                    else
                    {
                        return Result<string>.Failure(
                            string.Format("Product {0} does not exist", product.ProductName)
                        );
                    }
                }

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                {
                    return Result<string>.Failure("Failed to apply discount to product");
                }

                return Result<string>.Success("Discount application successfull");
            }
        }
    }
}
