namespace Application.Stripe.Charge
{
	public class ChargeResourceDto : ChargeResourceDtoAbstract
	{
		public string StripeName { get; set; }
		public string PhoneNumber { get; set; }
	}
}
