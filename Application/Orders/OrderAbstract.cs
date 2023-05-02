namespace Application.Orders
{
	public abstract class OrderAbstract
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DateOrdered { get; set; }
        public decimal TotalAmount { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal AmountDue { get; set; }
        public string OrderState { get; set; }
    }
}
