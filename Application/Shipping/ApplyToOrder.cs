using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Shipping
{
	public class ApplyToOrder
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid OrderId { get; set; }
			public Guid ShippingDetailId { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly AppDataContext _context;

			public Handler(AppDataContext context)
			{
				_context = context;
			}

			public async Task<Result<Unit>> Handle(
				Command request,
				CancellationToken cancellationToken
			)
			{
				var shippingDetail = _context.ShipingDetails
					.AsNoTracking()
					.First( s => s.ShippingDetailsId == request.ShippingDetailId);

				if(shippingDetail == null )
					return Result<Unit>.Failure("Detail does not exist");

				var orderToUpdate = _context.Orders.Find(request.OrderId);
				orderToUpdate.ShippingDetailsId = shippingDetail.ShippingDetailsId;
				_context.Orders.Update(orderToUpdate);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (!success)
					return Result<Unit>.Failure("Unable to add new shippingDetails");

				return Result<Unit>.Success(new Unit());
			}
		}
	}
}
