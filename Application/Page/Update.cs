using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Page
{
    public class Update
    {
        public class Command : IRequest<Result<PageDto>>
        {
            public PageUpdateParam PageUpdateParam { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<PageDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PageDto>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var pageToUpdate = _context.Pages
                    .Include(p => p.Store)
                    .FirstOrDefault(p => p.PageId == request.PageUpdateParam.PageId);
                if (pageToUpdate == null)
                    return Result<PageDto>.Failure("template does not exist");

                try
                {
                    pageToUpdate.Address = request.PageUpdateParam.Address;
                    pageToUpdate.PageCategory = request.PageUpdateParam.PageCategory;
                    pageToUpdate.PageNumber = request.PageUpdateParam.PageNumber;
                    pageToUpdate.Heading = request.PageUpdateParam.Heading;
                    pageToUpdate.SubHeading = request.PageUpdateParam.SubHeading;
                    pageToUpdate.MainColor = request.PageUpdateParam.MainColor;
                    pageToUpdate.SubColor = request.PageUpdateParam.SubColor;
                    pageToUpdate.FooterText = request.PageUpdateParam.FooterText;
                    pageToUpdate.InstagramLink = request.PageUpdateParam.InstagramLink;
                    pageToUpdate.FacebookLink = request.PageUpdateParam.FacebookLink;
                    pageToUpdate.TwitterLink = request.PageUpdateParam.TwitterLink;
                    pageToUpdate.PhoneNumber = request.PageUpdateParam.PhoneNumber;
                    pageToUpdate.HeroImage = request.PageUpdateParam.HeroImage;
                    pageToUpdate.Logo = request.PageUpdateParam.Logo;

                    _context.Pages.Update(pageToUpdate);
                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var pageToReturn = _mapper.Map<PageDto>(pageToUpdate);

                        return Result<PageDto>.Success(pageToReturn);
                    }

                    return Result<PageDto>.Failure("Page update failed");
                }
                catch (Exception ex)
                {
                    return Result<PageDto>.Failure(ex.Message);
                }
            }
        }
    }
}
