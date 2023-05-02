using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class Cart
    {
        public class Query : IRequest<Result<List<OrderDto>>>
        {
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
                var customer = _context.Customers.Any(c => c.Id == request.CustomerId);

                if (!customer)
                    return Result<List<OrderDto>>.Failure("Customer does not exist");

				var cartt = _context.Orders
					.Include(o => o.Customer)
					.Include(o => o.Purchases)
						.ThenInclude(p => p.Product)
					.Where(o =>o.CustomerId == request.CustomerId)
					.AsQueryable();

				if(request.Cart)
					cartt = cartt.Where(o =>
						o.OrderState == Domain.OrderStates.processing
					);
                
                var cartTosend = await cartt.ToListAsync();
                var cart = _mapper.Map<List<OrderDto>>(cartt);

                if (cart == null)
                    return Result<List<OrderDto>>.Failure("Customer Cart is Empty");

                return Result<List<OrderDto>>.Success(cart);
            }
        }
    }
}
