using Application.Interfaces;
using Application.Stripe;
using Application.Stripe.Bank;
using Application.Stripe.Card;
using Application.Stripe.Charge;
using Domain;
using Stripe;
using Charge = Stripe.Charge;

namespace Infrastructure.Stripe
{
	public class StripeService : IStripeService
    {
        private readonly TokenService _tokenService;
        private readonly CustomerService _customerService;
        private readonly ChargeService _chargeService;

		public StripeService(
            TokenService tokenService,
            CustomerService customerService,
            ChargeService chargeService
		)
        {
            _tokenService = tokenService;
            _customerService = customerService;
            _chargeService = chargeService;
		}

		public async Task<CustomerResource> CreateCardCustomer(
			CreateCardCustomerParam param,
            CancellationToken cancellationToken
        )
        {
            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = param.Card.Name,
                    Number = param.Card.Number,
                    ExpYear = param.Card.ExpiryYear,
                    ExpMonth = param.Card.ExpiryMonth,
                    Cvc = param.Card.Cvc
                }
            };

            var token = await _tokenService.CreateAsync(tokenOptions, null, cancellationToken);

			if(token.StripeResponse.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new Exception(message: token.StripeResponse.Content);
			}

            var customerOptions = new CustomerCreateOptions
            {
                Email = param.Email,
                Name = param.Name,
                Source = token.Id
            };
            var customer = await _customerService.CreateAsync(
                customerOptions,
                null,
                cancellationToken
            );

			if (customer.StripeResponse.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new Exception(message: customer.StripeResponse.Content);
			}
			return new CustomerResource(customer.Id, customer.Email, customer.Name);
        }

		public async Task<CustomerResource> CreateBankCustomer(
		   CreateBankCustomerParam param,
		   CancellationToken cancellationToken
		)
		{
			var tokenOptions = new TokenCreateOptions
			{
				BankAccount = new TokenBankAccountOptions
				{
					AccountHolderName = param.BankDetails.AccountHolderName,
					AccountHolderType = param.BankDetails.AccountHolderType,
					AccountNumber = param.BankDetails.AccountNumber,
					Country = param.BankDetails.Country,
					Currency = param.BankDetails.Currency
				}
			};

			var token = await _tokenService.CreateAsync(tokenOptions, null, cancellationToken);

			if (token.StripeResponse.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new Exception(message: token.StripeResponse.Content);
			}

			var customerOptions = new CustomerCreateOptions
			{
				Email = param.Email,
				Name = param.Name,
				Source = token.Id,
			};
			var customer = await _customerService.CreateAsync(
				customerOptions,
				null,
				cancellationToken
			);

			if (customer.StripeResponse.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new Exception(message: customer.StripeResponse.Content);
			}

			return new CustomerResource(customer.Id, customer.Email, customer.Name);
		}


		public async Task<ChargeResource> CreateCharge(
            CreateChargeParam Param,
            CancellationToken cancellationToken
        )
        {
            var chargeOptions = new ChargeCreateOptions
            {
                Currency = Param.Currency,
                Amount = Param.Amount,
                ReceiptEmail = Param.ReceiptEmail,
                Customer = Param.CustomerStripeId,
                Description = Param.Description,
            };

            var charge = await _chargeService.CreateAsync(chargeOptions, null, cancellationToken);

			if (charge.StripeResponse.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new Exception(message: charge.StripeResponse.Content);
			}

			return new ChargeResource(
                charge.Id,
                charge.Currency,
                charge.Amount,
                charge.CustomerId,
                charge.ReceiptEmail,
                charge.Description
            );
        }

		

		public async  Task<object> GetCustomerDetail(string stripeCustomerId)
		{
			var detail = await _customerService.GetAsync(stripeCustomerId);

			if(detail.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
			{
				if (detail.Object == "card")
					return new CardDetailDto(detail.Name, detail.Phone, detail.Description);

				return new BankDetailDto(detail.Name, detail.Email);
			}

			throw new Exception(message:detail.StripeResponse.Content);

		}

		public async Task<object> DeleteCustomer(string stripeCustomerId)
		{
			var detail = await _customerService.DeleteAsync(stripeCustomerId);

			if (detail.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
			{
				if (detail.Object == "card")
					return new CardDetailDto(detail.Name, detail.Object, detail.Description);

				return new BankDetailDto(detail.Object, detail.Email);
			}

			throw new Exception(message: detail.StripeResponse.Content);

		}

		public async Task<CustomerResource> Update(CustomerUpdateParam param, string stripeCustomerId)
		{
				var customerUpdateOptions = new CustomerUpdateOptions
				{
					
				};
				var customer = await _customerService.UpdateAsync(
					stripeCustomerId,
					customerUpdateOptions
				);

				return new CustomerResource(customer.Id, customer.Email, customer.Name);
		}

		public async Task<StripeList<Charge>> GetAllCharges()
		{
			var options = new ChargeListOptions
			{
					
			};

			var service = new ChargeService();

			StripeList<Charge> charges = await service.ListAsync(options);

			return charges;
		}
		public async Task<InternalChargeResourceDto> GetCharge(string chargeId)
		{
			var service = new ChargeService();
			Charge charge = await service.GetAsync(chargeId);

			var chargeDto = new InternalChargeResourceDto
			{
				Method = charge.PaymentMethodDetails.Card.Brand,
				AmountPaid = charge.Amount,
				StripeCustomerId = charge.CustomerId
			};
			
			return chargeDto;
		}

		public async Task<ChargeResourceDto> GetCardOrBankDetail(string chargeId)
		{
			var service = new CustomerService();
			var customer = await service.GetAsync(chargeId);

			var chargeDto = new ChargeResourceDto
			{
				StripeName = customer.Name,
				PhoneNumber = customer.Phone
			};
			return chargeDto;
		}




	}
}
