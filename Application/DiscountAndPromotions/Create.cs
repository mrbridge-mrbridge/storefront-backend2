using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.DiscountAndPromotions
{
    public class Create
    {
        public class Command : IRequest<Result<DiscountDto>>
        {
            public CreateParam CreateParam { get; set; }
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
                var store = _context.Stores.Find(request.CreateParam.StoreId);

                if (store == null)
                {
                    return Result<DiscountDto>.Failure("Store does no exist");
                }

                Discount discount = _mapper.Map<Discount>(request.CreateParam);
                _context.Discounts.Add(discount);

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                {
                    return Result<DiscountDto>.Failure("Store does no exist");
                }

                var discountDto = _mapper.Map<DiscountDto>(discount);

                return Result<DiscountDto>.Success(discountDto);
            }
        }
    }
}
