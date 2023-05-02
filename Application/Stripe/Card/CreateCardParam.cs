namespace Application.Stripe.Card
{
	public record CreateCardParam
	(
		string Name,
		string Number,
		string ExpiryYear,
		string ExpiryMonth,
		string Cvc
	);
}
