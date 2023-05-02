using Application.Core;
using Application.Purchases;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DiscountAndPromotions
{
    public class CalculateOnPurchase
    {
        public class Command : IRequest<Result<PurchaseDto>>
        {
            public Guid ProductId { get; set; }
            public Guid OrderId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<PurchaseDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<PurchaseDto>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    var purchaseToUpdate = _context.Purchases
                        .Include(p => p.Product)
                        .First(
                            p => p.ProductId == request.ProductId && p.OrderId == request.OrderId
                        );

                    if (purchaseToUpdate == null)
                        return Result<PurchaseDto>.Failure("Purchase does not exist");

                    var productDiscount = _context.Discounts.Find(
                        purchaseToUpdate.Product.DiscountId
                    );

                    if (
                        productDiscount != null
                        && productDiscount.Expires.ToUniversalTime() >= DateTime.UtcNow
                    )
                    {
                        purchaseToUpdate.Discount = productDiscount.Rate;
                    }

                    _context.Purchases.Update(purchaseToUpdate);

                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    var purchaseDto = _mapper.Map<PurchaseDto>(purchaseToUpdate);

                    if (success)
                        return Result<PurchaseDto>.Success(purchaseDto);

                    return Result<PurchaseDto>.Failure("Purchase failed");
                }
                catch (Exception ex)
                {
                    return Result<PurchaseDto>.Failure(ex.Message);
                }
            }
        }
    }
}
