using System;
namespace Domain
{
	public class Store
	{
		public Guid StoreId { get; set; }
		public Guid MerchantId { get; set; }
        public Merchant Merchant { get; set; }
		public ICollection<Page> Pages { get; set; } = new List<Page>();
        public string StoreName { get; set; }
		public string Currency {get; set;}
		public string CurrencySymbol {get; set;}
		public int NumberOfViews {get; set;}
		public ICollection<Product> Inventory { get; set; }
		public ICollection<ShippingDetails> ShipingDetails { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public ICollection<CreditCardDetail> CreditCards { get; set;}

	}
}
