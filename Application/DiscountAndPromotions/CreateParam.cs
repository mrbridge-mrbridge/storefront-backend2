namespace Application.DiscountAndPromotions
{
	public class CreateParam : DiscountAbstract
	{
		public Guid StoreId { get; set; }
		public DateTime Expires { get; set; }

	}
}
