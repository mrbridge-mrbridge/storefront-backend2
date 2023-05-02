using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stripe.Card
{
	public class CreateCardCustomer
	{
		public class Command : IRequest<Result<string>>
		{
			public CreateCardCustomerParam CreateCardCustomerParam { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<string>>
		{
			private readonly IStripeService _stripeService;
			private readonly AppDataContext _context;

			public Handler(IStripeService stripeService, AppDataContext context)
			{
				_stripeService = stripeService;
				_context = context;
			}

			public async Task<Result<string>> Handle(
				Command request,
				CancellationToken cancellationToken
			)
			{
				var customer = _context.Customers
					.Include(c => c.CreditCardDetails)
					.First(c => c.Id == request.CreateCardCustomerParam.CustomerId);

				if ( customer == null )
				{
					return Result<string>.Failure("customer does not exist");
				}

				try
				{
					var stripeCustomerResource = await _stripeService.CreateCardCustomer(
						request.CreateCardCustomerParam,
						cancellationToken
					);
			
					CreditCardDetail creditCardDetail = new()
					{
						StoreId = request.CreateCardCustomerParam.StoreId,
						StripeId = stripeCustomerResource.CustomerStripeId,
						Method = "card"
					};

					customer.CreditCardDetails.Add(creditCardDetail);

					_context.Update(customer);

					var success = await _context.SaveChangesAsync(cancellationToken) > 0;

					if(!success)
					{
						return Result<string>.Failure("Could not save card");
					}

					return Result<string>.Success("Card creation succeeded");
				}
				catch (Exception ex)
				{
					return Result<string>.Failure(ex.Message);
				}
			}
		}
	}
}
