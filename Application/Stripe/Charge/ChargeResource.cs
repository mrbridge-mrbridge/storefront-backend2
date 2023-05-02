namespace Application.Stripe.Charge
{
	public record ChargeResource
	(
		string ChargeId,
		string Currency,
		long Amount,
		string CustomerStripeId,
		string ReceiptEmail,
		string Description
	);
}
