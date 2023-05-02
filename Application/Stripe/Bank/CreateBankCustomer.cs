using Application.Core;
using Application.Interfaces;
using Application.Stripe.Card;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stripe.Bank
{
	public class CreateBankCustomer
	{
		public class Command : IRequest<Result<string>>
		{
			public CreateBankCustomerParam CreateBankCustomerParam { get; set; }
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
					.First(c => c.Id == request.CreateBankCustomerParam.CustomerId);

				if (customer == null)
				{
					return Result<string>.Failure("customer does not exist");
				}

				try
				{
					var stripeCustomerResource = await _stripeService.CreateBankCustomer(
						request.CreateBankCustomerParam,
						cancellationToken
					);

					CreditCardDetail creditCardDetail = new()
					{
						StoreId = request.CreateBankCustomerParam.StoreId,
						StripeId = stripeCustomerResource.CustomerStripeId,
						Method = "bank"
					};

					customer.CreditCardDetails.Add(creditCardDetail);

					_context.Customers.Update(customer);
					var success = await _context.SaveChangesAsync(cancellationToken) > 0;

					if (!success)
					{
						return Result<string>.Failure("Could not save bank account");
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
