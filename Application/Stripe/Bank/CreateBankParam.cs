namespace Application.Stripe.Bank
{
	public record CreateBankParam
		(
		string Country,
		string Currency,
		string AccountHolderName,
		string AccountHolderType,
		string RoutingNumber,
		string AccountNumber
		);
}
