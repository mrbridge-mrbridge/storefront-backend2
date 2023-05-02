namespace Application.Stripe.Charge
{
	public class CreateChargeParam : ChargeAbstract
	{
		public string ReceiptEmail { get; set; }
		public virtual string CustomerStripeId { get; set; }
		public string Currency { get; set; }
	}
}
