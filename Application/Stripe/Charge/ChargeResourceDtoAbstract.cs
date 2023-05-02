namespace Application.Stripe.Charge
{
	public abstract class  ChargeResourceDtoAbstract
	{
		public string Method { get; set; }
		public float AmountPaid { get; set; }
	}
}
