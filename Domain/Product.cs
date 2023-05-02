namespace Domain;
public class Product
{
	public Guid ProductId { get; set; }
	public string ProductName { get; set; }
	public string ProductDescription { get; set; }
	public string ProductCategory { get; set; }
	public string UnitOfMeasurement { get; set; }
	public decimal Quantity { get; set; }
	public bool Publish { get; set; }
	public decimal UnitPrice { get; set; }
	public Guid StoreId { get; set; }
	public Store Store { get; set; }
	public List<Purchase> Purchases { get; set; }
	public ICollection<CustomerReview> Reviews { get; set; }
	public ICollection<ProductPhoto> ProductPhotos { get; set; }
	public Guid DiscountId { get; set; }
}
