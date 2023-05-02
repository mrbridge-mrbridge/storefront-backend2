namespace Domain
{
	public class Page
    {
        public Guid PageId { get; set; }
        public Guid StoreId { get; set; }
        public string PageCategory { get; set; }
        public int PageNumber { get; set; }
        public string MainHeaderTextSize { get; set; }
        public string SubHeaderTextsize { get; set; }
        public string HeroImage { get; set; }
        public string Logo { get; set; }
        public string Heading{ get; set; }
        public string SubHeading { get; set; }
        public string MainColor{ get; set; }
        public string SubColor { get; set; }
        public string FooterText{ get; set; }
        public string Address{ get; set; }
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string PhoneNumber { get; set; }
        public bool Publish { get; set; }
        public Store Store { get; set; }
        public ICollection<PagePhoto> PagePhotos { get; set; }

    }
}