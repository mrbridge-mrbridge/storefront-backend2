namespace Domain
{
	public class ReviewReply
    {
        public Guid MerchantId { get; set; }
		public Guid CustomerId { get; set; }
		public Guid ProductId { get; set; }
		public Merchant Merchant { get; set; }
        public string Reply { get; set; }
        public CustomerReview Review { get; set; }
        public DateTime DateReplied { get; set; }

    }
}
