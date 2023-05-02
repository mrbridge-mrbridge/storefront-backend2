using Application.Core;
using Application.Purchases;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Shipping
{
    public class Create
    {
        public class Command : IRequest<Result<ShippingDto>>
        {
            public ShippingParam ShippingParam { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<ShippingDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ShippingDto>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var newshippingDetail = _mapper.Map<ShippingDetails>(request.ShippingParam);

                _context.ShipingDetails.Add(newshippingDetail);

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                    return Result<ShippingDto>.Failure("Unable to add new shippingDetails");

				var shipping = _mapper.Map<ShippingDto>(newshippingDetail);

                return Result<ShippingDto>.Success(shipping);
            }
        }
    }
}
