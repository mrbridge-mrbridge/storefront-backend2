using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.DiscountAndPromotions
{
    public class UpDate
    {
        public class Command : IRequest<Result<DiscountDto>>
        {
            public CreateParam CreateParam { get; set; }
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
                    return Result<DiscountDto>.Failure("Discount does not exist");
                }

                discount.Rate = request.CreateParam.Rate;
                discount.Name = request.CreateParam.Name;
                discount.Expires = request.CreateParam.Expires;

                _context.Discounts.Update(discount);

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                {
                    return Result<DiscountDto>.Failure("Store does not exist");
                }

                var discountDto = _mapper.Map<DiscountDto>(discount);

                return Result<DiscountDto>.Success(discountDto);
            }
        }
    }
}
