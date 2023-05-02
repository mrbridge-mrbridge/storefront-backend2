namespace Domain
{
	public class CreditCardDetail
    {
        public Guid CustomerId { get; set; }
        public Guid StoreId { get; set; }
		public string Method { get; set; }
        public string StripeId { get; set; }
		public Store Store { get; set; }
        public Customer Customer { get; set; }
    }
}
