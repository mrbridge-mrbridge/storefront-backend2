using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Data.Common;

namespace Application.Page
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid PageId { get; set; }
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
				var page = _context.Pages.Include(p => p.PagePhotos).First(p => p.PageId == request.PageId);


				if (page == null) { return Result<Unit>.Failure("page does not exist"); }

				_context.Remove(page);

				var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				if (!success) return Result<Unit>.Failure("Could not remove page");

				return Result<Unit>.Success(new Unit());

			}
		}
	}
}
