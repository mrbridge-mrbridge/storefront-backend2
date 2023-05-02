using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Orders
{
	public class TagProcessing
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid OrderId { get; set; }
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
				
				var orderToUpdate = _context.Orders
					.Find(request.OrderId);

				if (orderToUpdate == null)
					return Result<Unit>.Failure("Order does not exist");

				orderToUpdate.OrderState = OrderStates.processing;

				_context.Orders.Update(orderToUpdate);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (success)
				{
					return Result<Unit>.Success(new Unit());
				}

				return Result<Unit>.Failure("Order state change failed");
				
			}
		}
	}
}
