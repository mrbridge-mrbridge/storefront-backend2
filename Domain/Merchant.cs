namespace Domain
{
	public class Merchant: User
	{
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string StreetName { get; set; }
        public ICollection<Store> Stores { get; set; }
        public ICollection<ReviewReply> ReviewReplies { get; set; } = new List<ReviewReply>();
    }
}
