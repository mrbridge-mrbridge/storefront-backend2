namespace Application.Product
{
	public abstract class ProductAbstract
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int Quantity { get; set; }
        public bool Publish { get; set; }
		public decimal UnitPrice { get; set; }

	}
}
