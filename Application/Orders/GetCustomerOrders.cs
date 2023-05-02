using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
	public class GetCustomerOrders
	{
		public class Query : IRequest<Result<List<OrderDto>>>
		{
			public Guid StoreId { get; set; }
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
				var store = await  _context.Products
					.Include(s => s.Purchases)
						.ThenInclude(p => p.Order)
					.Where(s => s.StoreId == request.StoreId)
					.ToListAsync();

				var storeOrder = store.SelectMany(o => o.Purchases
					.Select(p => p.Order))
					.AsQueryable();

				var unique = storeOrder.ToHashSet();
				var stor = unique.AsQueryable();

				if (store == null)
					return Result<List<OrderDto>>.Failure("Store has no orders");

				if (request.Cart)
					stor = stor.Where(o =>
						o.OrderState == OrderStates.processing
					);

				var cart = _mapper.Map<List<OrderDto>>(stor);

				if (cart == null)
					return Result<List<OrderDto>>.Failure("Customer Cart is Empty");

				return Result<List<OrderDto>>.Success(cart);
			}
		}
	}
}
