using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.DiscountAndPromotions
{
    public class Delete
    {
        public class Command : IRequest<Result<DiscountDto>>
        {
            public Guid DiscountId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<DiscountDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<DiscountDto>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var discount = _context.Discounts.Find(request.DiscountId);

                if (discount == null)
                {
                    return Result<DiscountDto>.Failure("Discount does no exist");
                }

                foreach (
                    Domain.Product product in _context.Products.Where(
                        p => p.StoreId == discount.StoreId
                    )
                )
                {
                    if (product.DiscountId == discount.DiscountId)
                    {
                        product.DiscountId = Guid.Empty;
                        _context.Products.Update(product);
                    }
                }

                _context.Discounts.Remove(discount);

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                {
                    return Result<DiscountDto>.Failure("Discount deletion failed");
                }

                var discountDto = _mapper.Map<DiscountDto>(discount);

                return Result<DiscountDto>.Success(discountDto);
            }
        }
    }
}
