using Application.Core;
using Application.Product;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ReviewReplies
{
    public class GetReply
    {
        public class Query : IRequest<Result<ProductDto>>
        {
            public ProductDto ProductDto { get; set; }
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
                foreach(var review in request.ProductDto.Reviews)
                {
                    ReviewReply reply = await _context.ReviewReplies.AsNoTracking().FirstOrDefaultAsync(r => r.CustomerId == review.CustomerId && r.ProductId == review.ProductId);

                    var replydto = _mapper.Map<ReplyDto>(reply);
                    review.ReviewReply = replydto;
                }

                // if (product == null)
                // {
                //     return Result<ProductDetail>.Failure("Product does not exist");
                // }

                // var productDetail = _mapper.Map<ProductDetail>(product);

                return Result<ProductDto>.Success(request.ProductDto);
            }
        }
    }
}