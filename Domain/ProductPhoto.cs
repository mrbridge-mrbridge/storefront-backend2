namespace Domain
{
	public class ProductPhoto: Photo
    {
        public Guid ProductId{get; set;}
        public Product Product {get; set;}
    }
}