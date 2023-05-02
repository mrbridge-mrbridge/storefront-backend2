namespace Domain
{
	public class Discount
	{
		public Guid DiscountId { get; set; }
		public Guid StoreId { get; set; }
		public string Name { get; set; }
		public DateTime Expires { get; set; }
		public decimal Rate { get; set; }
		public Store Store { get; set; }
	}
}
