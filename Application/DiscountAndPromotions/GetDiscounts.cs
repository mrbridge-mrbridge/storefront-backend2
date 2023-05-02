using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DiscountAndPromotions
{
    public class GetDiscounts
    {
        public class Command : IRequest<Result<List<DiscountDto>>>
        {
            public Guid StoreId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<List<DiscountDto>>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<DiscountDto>>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var discount = await _context.Discounts
                    .Where(d => d.StoreId == request.StoreId)
                    .ToListAsync(cancellationToken);

                if (discount == null)
                {
                    return Result<List<DiscountDto>>.Failure("No Discount exist");
                }

                var discountDto = _mapper.Map<List<DiscountDto>>(discount);

                return Result<List<DiscountDto>>.Success(discountDto);
            }
        }
    }
}
