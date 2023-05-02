using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Product
{
    public class Details
    {
        public class Query : IRequest<Result<ProductDto>>
        {
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ProductDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ProductDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var product = await _context.Products
                    .Include(p => p.ProductPhotos)
                    .Include(p => p.Reviews)
                    .ThenInclude(r => r.ReviewReply)
                    .FirstOrDefaultAsync(p => p.ProductId == request.ProductId);

                if (product == null)
                {
                    return Result<ProductDto>.Failure("Product does not exist");
                }

                var productDetail = _mapper.Map<ProductDto>(product);

                return Result<ProductDto>.Success(productDetail);
            }
        }
    }
}
