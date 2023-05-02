namespace Domain
{
	public class Order
    {
        public Guid OrderId {get; set;}
        public Guid CustomerId { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
        public DateTime DateOrdered { get; set; } = DateTime.UtcNow;
        public Customer Customer { get; set; }
        public OrderStates OrderState { get; set; }
        public Guid ShippingDetailsId {get; set; }
		public Charge Charge { get; set; }
    }

    public enum OrderStates
    {
        processing,
        shipping,
        delivered,
		cancelled
    }
}
