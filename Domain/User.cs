namespace Domain
{
	public class User{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Password { get; set; }
		public string OauthId { get; set; }
		public string Role { get; set; }
		public bool Activated { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}