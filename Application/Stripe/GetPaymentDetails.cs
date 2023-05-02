using Application.Core;
using Application.Interfaces;
using Application.Stripe.Charge;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stripe
{
	public class GetPaymentDetails
	{
		public class Query : IRequest<Result<List<ChargeResourceDto>>>
		{
			public Guid CustomerId { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<ChargeResourceDto>>>
		{
			private readonly AppDataContext _context;
			private readonly IStripeService _stripeService;

			public Handler(AppDataContext context, IStripeService stripeService)
			{
				_context = context;
				_stripeService = stripeService;
			}

			public async Task<Result<List<ChargeResourceDto>>> Handle(
				Query request,
				CancellationToken cancellationToken
			)
			{
				var customer = _context.Customers.Find(request.CustomerId);

				if (customer == null)
					return Result<List<ChargeResourceDto>>.Failure("Customer does not exist");

				List<string> chargeIds = new List<string>();
				var charges = _context.Charges.Include(c => c.Order).ToList();

				foreach(Domain.Charge charge in charges)
				{
					if (charge.Order.CustomerId == customer.Id)
						chargeIds.Add(charge.ChargeId);
				}

				if (chargeIds == null)
					return Result<List<ChargeResourceDto>>.Failure("Customer has no charges");


				List<InternalChargeResourceDto> stripeCharges = new List<InternalChargeResourceDto>();

				foreach (string chargeId in chargeIds)
				{
					var stripeCharge = await _stripeService.GetCharge(chargeId);
					stripeCharges.Add(stripeCharge);
				}

				List<ChargeResourceDto> Stripeinfo = new List<ChargeResourceDto>();

				foreach(InternalChargeResourceDto  dto in stripeCharges )
				{
					var stripeInfo = await _stripeService.GetCardOrBankDetail(dto.StripeCustomerId);
					stripeInfo.AmountPaid = dto.AmountPaid;
					stripeInfo.Method = dto.Method;
					Stripeinfo.Add(stripeInfo);
				}

				return Result<List<ChargeResourceDto>>.Success(Stripeinfo);
			}
		}
	}
}
