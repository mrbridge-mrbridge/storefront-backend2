using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Shipping
{
    public class GetShippingDetails
    {
        public class Query : IRequest<Result<List<ShippingDto>>>
        {
            public Guid CustomerId { get; set; }
            public Guid StoreId { get; set; }
			
        }

        public class Handler : IRequestHandler<Query, Result<List<ShippingDto>>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<ShippingDto>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var shippingDetails = _context.ShipingDetails.Where(
                    s =>
                        s.CustomerId == request.CustomerId
                )
					.AsQueryable();
				if (request.StoreId != Guid.Empty)
					shippingDetails = shippingDetails.Where(s => s.StoreId == request.StoreId);

				var shippingDetailDto = await shippingDetails
					.ProjectTo<ShippingDto>(_mapper.ConfigurationProvider)
					.ToListAsync(cancellationToken);

				if (shippingDetailDto == null)
                    return Result<List<ShippingDto>>.Failure("customer has no details yet");

                return Result<List<ShippingDto>>.Success(shippingDetailDto);
            }
        }
    }
}
