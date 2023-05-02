namespace Application.Stripe.Charge
{
	public class InternalChargeResourceDto : ChargeResourceDtoAbstract
	{
		public string StripeCustomerId { get; set; }
	}
}
