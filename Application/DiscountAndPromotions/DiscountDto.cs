namespace Application.DiscountAndPromotions
{
	public class DiscountDto : DiscountAbstract
	{
		public Guid DiscountId { get; set; }
		public string Expires { get; set; }
	}
}
