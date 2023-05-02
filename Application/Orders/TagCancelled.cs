using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
	public class TagCancelled
	{
		public class Command : IRequest<Result<List<OrderCancellationDto>>>
		{
			public Guid OrderId { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<List<OrderCancellationDto>>>
		{
			private readonly AppDataContext _context;

			public Handler(AppDataContext context)
			{
				_context = context;
			}

			public async Task<Result<List<OrderCancellationDto>>> Handle(
				Command request,
				CancellationToken cancellationToken
			)
			{

				var orderToUpdate = _context.Orders
					.Include(o => o.Purchases)
					.FirstOrDefault(o => o.OrderId == request.OrderId);

				if (orderToUpdate == null)
					return Result<List<OrderCancellationDto>>.Failure("Order does not exist");

				orderToUpdate.OrderState = OrderStates.cancelled;

				_context.Orders.Update(orderToUpdate);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				var result = new List<OrderCancellationDto>();

				foreach(Purchase purchase in orderToUpdate.Purchases)
				{
					result.Add(new OrderCancellationDto(orderToUpdate.OrderId, purchase.ProductId));
				}

				if (success)
				{
					return Result<List<OrderCancellationDto>>.Success(result);
				}

				return Result<List<OrderCancellationDto>>.Failure("Order state change failed");

			}
		}
	}
}
