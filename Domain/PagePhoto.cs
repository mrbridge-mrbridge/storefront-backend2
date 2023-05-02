namespace Domain
{
	public class PagePhoto: Photo
    {
        public Guid PageId{get; set;}
        public Page Page {get; set;}
    }
}