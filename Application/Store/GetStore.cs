using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Store
{
    public class GetStore
    {
        public class Query : IRequest<Result<List<GetStoreDto>>>
        {
            public Guid StoreId { get; set; }
            public Guid MerchantId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<GetStoreDto>>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<GetStoreDto>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var store = _context.Stores.Include(s => s.Pages).AsQueryable();

                if (store != null)
                {
                    if (request.MerchantId != Guid.Empty)
                        store = store.Where(s => s.MerchantId == request.MerchantId);

                    if (request.StoreId != Guid.Empty)
                    {
                        store = store.Where(s => s.StoreId == request.StoreId);
                    }

                    var stores = await store.ToListAsync();
                    
                    var storeDto = _mapper.Map<List<GetStoreDto>>(stores);

                    return Result<List<GetStoreDto>>.Success(storeDto);
                }

                return Result<List<GetStoreDto>>.Failure("Store does not exist");
            }
        }
    }
}
