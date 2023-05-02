using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class CustomerOrdersForAStore
    {
        public class Query : IRequest<Result<List<OrderDto>>>
        {
            public Guid StoreId { get; set; }
            public Guid CustomerId { get; set; }
            public bool Cart { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<OrderDto>>>
        {
            private readonly AppDataContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<OrderDto>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var cartt = _context.Orders
                     .Include(o => o.Customer)
                     .Include(o => o.Purchases.Where(p => p.Product.StoreId == request.StoreId))
                         .ThenInclude(p => p.Product)
                         .ThenInclude(ph => ph.ProductPhotos)
                     .Where(o => o.CustomerId == request.CustomerId)
                     .AsQueryable();
                if(request.Cart)
                    cartt.Where(o => o.OrderState == OrderStates.processing);

                var cart = await cartt.ToListAsync(cancellationToken);

                var cartToSend = _mapper.Map<List<OrderDto>>(cart);

                if (cartToSend == null)
                    return Result<List<OrderDto>>.Failure("Customer Cart is Empty");

                return Result<List<OrderDto>>.Success(cartToSend);
            }

        }
    }
}
