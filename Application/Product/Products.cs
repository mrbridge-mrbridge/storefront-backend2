using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Product
{
    public class Products
    {
        public class Query : IRequest<Result<List<ProductDto>>>
        {
            public Guid StoreId { get; set; }
            public string ProductName { get; set; }
            public string ProductCategory { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly AppDataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(AppDataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<List<ProductDto>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                IQueryable<Domain.Product> products = _dataContext.Products.Include(p => p.ProductPhotos).AsQueryable();

                if (request.StoreId != Guid.Empty)
                    products = products.Where(p => p.StoreId == request.StoreId);

                if (!string.IsNullOrEmpty(request.ProductName))
                    products = products.Where(
                        p => p.ProductName.ToLower().Contains(request.ProductName.ToLower())
                    );

                if (!string.IsNullOrEmpty(request.ProductCategory))
                    products = products.Where(
                        p => p.ProductCategory.ToLower().Contains(request.ProductCategory.ToLower())
                    );

                List<Domain.Product> productsToSend = await products.ToListAsync();

                List<ProductDto> productDto = _mapper.Map<List<ProductDto>>(productsToSend);

                return Result<List<ProductDto>>.Success(productDto);
            }
        }
    }
}
