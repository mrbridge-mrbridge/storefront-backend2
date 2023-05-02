using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product
{
	public class Delete
	{
		public class Command:IRequest<Result<Unit>>
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
				var product = _context.Products.Include(p => p.ProductPhotos).Include(p => p.Reviews).First(p => p.ProductId==request.ProductId);

				if (product == null) { return Result<Unit>.Failure("product does not exist"); }

				_context.Remove(product);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (!success) return Result<Unit>.Failure("Could not remove Product");

				return Result<Unit>.Success(new Unit());

			}
		}
	}
}
