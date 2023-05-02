using Application.Core;
using Application.Interfaces;
using Application.Photos;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Product
{
    public class AddPhoto
    {
        public class Command : IRequest<Result<PhotoUploadResult>>
        {
            public IFormFile ProductPhoto { get; set; }
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<PhotoUploadResult>>
        {
            private readonly AppDataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IPhotoAccessor photoAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
                _photoAccessor = photoAccessor;
            }

            public async Task<Result<PhotoUploadResult>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var product = _context.Products.Find(request.ProductId);

                if (product == null)
                    return Result<PhotoUploadResult>.Failure("Product does not exist");

                try
                {
                    var ProductPhoto = await _photoAccessor.AddPhoto(request.ProductPhoto);

                    ProductPhoto productPhoto = new ProductPhoto
                    {
                        Id = ProductPhoto.PublicId,
                        Url = ProductPhoto.Url,
                        ProductId = product.ProductId
                    };

                    _context.ProductPhotos.Add(productPhoto);

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var uploadResult = _mapper.Map<PhotoUploadResult>(productPhoto);

                        return Result<PhotoUploadResult>.Success(uploadResult);
                    }

                    return Result<PhotoUploadResult>.Failure("Productphoto upload failed");
                }
                catch (Exception ex)
                {
                    return Result<PhotoUploadResult>.Failure(ex.Message);
                }
            }
        }
    }
}
