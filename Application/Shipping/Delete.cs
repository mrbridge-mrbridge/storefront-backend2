using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Shipping
{
	public class Delete
	{
		public class Command : IRequest<Result<ShippingDto>>
		{
			public Guid ShippingDetailId { get; set; }
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
				var shippingDetailToDelete = _context.ShipingDetails
					.Find(request.ShippingDetailId);


				_context.ShipingDetails.Remove(shippingDetailToDelete);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (!success)
					return Result<ShippingDto>.Failure("Unable to add new shippingDetails");

				var shipping = _mapper.Map<ShippingDto>(shippingDetailToDelete);

				return Result<ShippingDto>.Success(shipping);
			}
		}
	}
}
