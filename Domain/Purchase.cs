namespace Domain
{
	public class Purchase
	{
		public Guid ProductId { get; set; }
		public Guid OrderId { get; set; }
		public Product Product { get; set; }
		public decimal QuantityPurchased { get; set; }
		public DateTime DatePurchased { get; set; } = DateTime.UtcNow;
		public Order Order {get; set; }
		public decimal? Discount { get; set; }
		
	}

}
