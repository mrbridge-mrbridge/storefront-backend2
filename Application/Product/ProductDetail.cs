using Application.Photos;
using Application.Purchases;
using Application.ReviewReplies;
using Application.Reviews;
using Domain;

namespace Application.Product
{
    public class ProductDetail : ProductAbstract
	{
		public string DefaultImage { get; set; }
		public ICollection<ReviewInternalDto> Reviews { get; set; } = new List<ReviewInternalDto>();
		public ICollection<PhotoUploadResult> ProductPhotos { get; set; }
	}
}
