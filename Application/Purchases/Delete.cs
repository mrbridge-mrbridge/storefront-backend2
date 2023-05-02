using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Purchases
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid ProductId { get; set; }
			public Guid OrderId { get; set; }
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
				var purchaseToDelete = _context.Purchases
					.Find(request.OrderId, request.ProductId);

				if (purchaseToDelete == null)
					return Result<Unit>.Failure("Purchase does not exist");

				_context.Remove(purchaseToDelete);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (!success)
					return Result<Unit>.Failure("Could not remove Purchase");

				return Result<Unit>.Success(new Unit());

			}
		}
	}
}
