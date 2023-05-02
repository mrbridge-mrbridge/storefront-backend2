using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Product
{
    public class GetStoreProductCategories
    {
        public class Query : IRequest<Result<HashSet<string>>>
        {
            public Guid StoreId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<HashSet<string>>>
        {
            private readonly AppDataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(AppDataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<HashSet<string>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                IQueryable<Domain.Product> products = _dataContext.Products.Where(p => p.StoreId == request.StoreId).AsQueryable();

                HashSet<string> categories = products.Select(p => p.ProductCategory).ToHashSet();

                return Result<HashSet<string>>.Success(categories);
            }
        }
    }
}