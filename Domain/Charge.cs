namespace Domain
{
	public class Charge
	{
		public string ChargeId { get; set; }
		public Guid OrderId { get; set; }
		public	Order Order { get; set; }
	}
}
