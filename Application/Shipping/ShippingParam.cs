using Application.Shipping;

namespace Application.Purchases
{
	public class ShippingParam : ShippingDetailsAbstract
    {
		public Guid StoreId { get; set; }
		public Guid CustomerId { get; set; }
	}
}
