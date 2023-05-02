using Application.Core;
using Application.Interfaces;
using Application.Photos;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Page
{
    public class AddLogoPhoto
    {
        public class Command : IRequest<Result<PhotoUploadResult>>
        {
            public IFormFile LogoPhoto { get; set; }
            public Guid PageId { get; set; }
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
                var page = _context.Pages.Find(request.PageId);
                if (page == null)
                    return Result<PhotoUploadResult>.Failure("Page does not exist");

                try
                {
                    var logo = await _photoAccessor.AddPhoto(request.LogoPhoto);
                    page.Logo = logo.PublicId;

                    PagePhoto logoPhoto = new PagePhoto
                    {
                        Id = logo.PublicId,
                        Url = logo.Url,
                        PageId = page.PageId
                    };

                    _context.PagePhotos.Add(logoPhoto);

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var result = _mapper.Map<PhotoUploadResult>(logoPhoto);
                        return Result<PhotoUploadResult>.Success(result);
                    }
                    return Result<PhotoUploadResult>.Failure("Could not upload logo photo");
                }
                catch (Exception ex)
                {
                    return Result<PhotoUploadResult>.Failure(ex.Message);
                }
            }
        }
    }
}
