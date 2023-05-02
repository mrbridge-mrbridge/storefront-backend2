using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Store
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid ProductId { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly AppDataContext _context;

			public Handler(AppDataContext context)
			{
				_context = context;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var product = _context.Stores
					.Include(s => s.Inventory)
					.Include(s => s.ShipingDetails)
					.Include(s => s.Pages)
					.Include(s => s.Discounts)
					.First(s => s.StoreId == request.ProductId);

				if (product == null) { return Result<Unit>.Failure("product does not exist"); }

				_context.Remove(product);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (!success) return Result<Unit>.Failure("Could not remove Product");

				return Result<Unit>.Success(new Unit());

			}
		}
	}
}
