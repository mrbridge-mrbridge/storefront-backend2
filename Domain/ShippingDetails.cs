namespace Domain
{
	public class ShippingDetails
    {
        public Guid ShippingDetailsId { get; set; }
        public Guid StoreId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public string Address { get; set; }
        public string StreetName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
