using Application.Stripe;
using Application.Stripe.Charge;

namespace Application.Interfaces
{
    public interface IStripeService
    {
        Task<CustomerResource> CreateCardCustomer(
            Stripe.Card.CreateCardCustomerParam resource,
            CancellationToken cancellationToken
        );
        Task<CustomerResource> CreateBankCustomer(
            Stripe.Bank.CreateBankCustomerParam resource,
            CancellationToken cancellationToken
        );
        Task<ChargeResource> CreateCharge(
            CreateChargeParam resource,
            CancellationToken cancellationToken
        );
        Task<InternalChargeResourceDto> GetCharge(string chargeId);
        Task<ChargeResourceDto> GetCardOrBankDetail(string chargeId);
    }
}
