using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Page
{
    public class Create
    {
        public class Command : IRequest<Result<PageDto>>
        {
            public CreatePageParam CreatePageParam { get; set; }
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
                var store = _context.Stores.Find(request.CreatePageParam.StoreId);

                if (store == null)
                    return Result<PageDto>.Failure("store does not exist");

                try
                {
                    Domain.Page newPage = _mapper.Map<Domain.Page>(request.CreatePageParam);
                    newPage.HeroImage = "";
                    newPage.Logo = "";

                    _context.Pages.Add(newPage);
                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (success)
                    {
                        var pageToReturn = _context.Pages
                            .Include(p => p.Store)
                            .FirstOrDefault(
                                p =>
                                    p.PageCategory == newPage.PageCategory
                                    && p.PageNumber == newPage.PageNumber
                            );

                        var PageToReturn = _mapper.Map<PageDto>(pageToReturn);

                        return Result<PageDto>.Success(PageToReturn);
                    }

                    return Result<PageDto>.Failure("Could not create Page");
                }
                catch (Exception ex)
                {
                    return Result<PageDto>.Failure(ex.Message);
                }
            }
        }
    }
}
