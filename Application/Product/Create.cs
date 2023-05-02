using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Product
{
    public class Create
    {
        public class Command : IRequest<Result<ProductDto>>
        {
            public ProductCreateParam ProductCreateParam { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<ProductDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ProductDto>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var product = _mapper.Map<Domain.Product>(request.ProductCreateParam);

                _context.Products.Add(product);

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

				var ProductToReturn = _mapper.Map<ProductDto>(product);

				if (success)
					return Result<ProductDto>.Success(ProductToReturn);

				return Result<ProductDto>.Failure("Product addition failed");
			}
        }
    }
}
