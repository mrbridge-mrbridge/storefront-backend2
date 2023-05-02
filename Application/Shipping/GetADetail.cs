using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Shipping
{
    public class GetADetail
    {
        public class Query : IRequest<Result<ShippingDto>>
        {
            public Guid ShippingDetailId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ShippingDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ShippingDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var shippingDetails = await _context.ShipingDetails.FindAsync(
                    request.ShippingDetailId
                );

                if (shippingDetails == null)
                    return Result<ShippingDto>.Failure("Shipping detail does not exist");

                var details = _mapper.Map<ShippingDto>(shippingDetails);

                return Result<ShippingDto>.Success(details);
            }
        }
    }
}
