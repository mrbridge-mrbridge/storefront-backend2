using Application.Photos;
using Domain;
using Application.ReviewReplies;
using Application.Reviews;
namespace Application.Product
{
	public class ProductDto : ProductAbstract
    {
        public Guid ProductId { get; set; }
        public string StoreName { get; set; }
        public string DefaultImage { get; set; }
        public ICollection<ReviewInternalDto> Reviews { get; set; } = new List<ReviewInternalDto>();
        public List<PhotoUploadResult> ProductPhotos { get; set; }
    }
}