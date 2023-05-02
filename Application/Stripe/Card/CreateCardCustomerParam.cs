namespace Application.Stripe.Card
{
	public record CreateCardCustomerParam(
		string Email,
		string Name,
		Guid StoreId,
		Guid CustomerId,
		CreateCardParam Card
	);
}
