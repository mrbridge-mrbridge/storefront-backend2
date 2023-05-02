using Application.Core;
using Application.Purchases;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Shipping
{
	public class Update
	{
		public class Command : IRequest<Result<ShippingDto>>
		{
			public UpdateParam UpdateParam { get; set; }
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
				var shippingDetailToUpdate = _context.ShipingDetails.Find(request.ShippingDetailId);

				shippingDetailToUpdate.Address = request.UpdateParam.Address;
				shippingDetailToUpdate.PhoneNumber = request.UpdateParam.PhoneNumber;
				shippingDetailToUpdate.StreetName = request.UpdateParam.StreetName;

				_context.ShipingDetails.Update(shippingDetailToUpdate);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (!success)
					return Result<ShippingDto>.Failure("Unable to add new shippingDetails");

				var shipping = _mapper.Map<ShippingDto>(shippingDetailToUpdate);

				return Result<ShippingDto>.Success(shipping);
			}
		}
	}
}
