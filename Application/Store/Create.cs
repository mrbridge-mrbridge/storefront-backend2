using System.Net.Cache;
using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Store
{
	public class Create
    {
        public class Command: IRequest<Result<StoreDto>>
        {
            public CreateHomePageParam CreateHomePageParam { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<StoreDto>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<StoreDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                try
                {
                    Domain.Store newTem = new()
                    {
						MerchantId = request.CreateHomePageParam.CreateStoreParam.MerchantId,
						StoreName = request.CreateHomePageParam.CreateStoreParam.storeName,
						Currency = "EUR",
						CurrencySymbol = "€",
					};

                    _context.Stores.Add(newTem);
                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if(success)
                    {
                    var newstore = _mapper.Map<StoreDto>(newTem);
                    return Result<StoreDto>.Success(newstore);

                    }
                    return Result<StoreDto>.Failure("Store creation failed");

                } catch(Exception ex)
                {
                    return Result<StoreDto>.Failure(ex.Message);
                }

            }
        }
    }
}
