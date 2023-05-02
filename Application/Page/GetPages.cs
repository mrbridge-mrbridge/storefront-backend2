using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Page
{
    public class GetPages
    {
        public class Query : IRequest<Result<List<PageDto>>>
        {
            public GetPageParam GetPageParam { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<PageDto>>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<PageDto>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var page = _context.Pages
                    .Include(p => p.PagePhotos)
                    .Where(t => t.StoreId == request.GetPageParam.StoreId)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.GetPageParam.PageCategory))
                {
                    page = page.Where(d => d.PageCategory == request.GetPageParam.PageCategory);
                }

                if (page == null)
                {
                    return Result<List<PageDto>>.Failure("This category does not exist");
                }

                var pageToReturn = await page.ToListAsync();
                var toReturn = _mapper.Map<List<PageDto>>(pageToReturn);

                return Result<List<PageDto>>.Success(toReturn);
            }
        }
    }
}
