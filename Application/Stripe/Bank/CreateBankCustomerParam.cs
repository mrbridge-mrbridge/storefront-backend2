namespace Application.Stripe.Bank
{
	public record CreateBankCustomerParam
		(
		 string Email,
		string Name,
		Guid StoreId,
		Guid CustomerId,
		CreateBankParam BankDetails
		);
}
