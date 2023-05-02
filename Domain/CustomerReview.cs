namespace Domain
{
	public class CustomerReview
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime DateCommented { get; set; } = DateTime.UtcNow;
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public ReviewReply ReviewReply { get; set; }
    }
}
