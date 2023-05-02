using System;
namespace Domain
{
	public class Customer: User
	{
		public Guid OrderId { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<ShippingDetails> ShipingDetails { get; set; }
		public ICollection<CustomerReview> ProductReviews {get; set;} = new List<CustomerReview>();
		public ICollection<CreditCardDetail> CreditCardDetails { get; set; }
	}
}
