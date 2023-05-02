namespace Application.Store
{
	public class StoreDto
    {
        public Guid StoreId { get; set; }
        public Guid MerchantId { get; set; }
        public string StoreName { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
    }
}