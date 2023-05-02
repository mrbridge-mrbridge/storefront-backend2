using Application.Core;
using MediatR;
using Persistence;

namespace Application.Stripe.Bank
{
	public class Details
	{
		public class Query : IRequest<Result<Object>>
		{
			public Guid CustomerId { get; set; }
			public string Method { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<Object>>
		{
			public Handler(AppDataContext context)
			{

			}

			public Task<Result<object>> Handle(Query request, CancellationToken cancellationToken)
			{
				throw new NotImplementedException();
			}
		}
	}
}
