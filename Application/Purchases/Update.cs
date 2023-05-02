using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Purchases
{
	public class Update
	{
		public class Command : IRequest<Result<PurchaseDto>>
		{
			public PurchaseUpdateParam PurchaseCreateParam { get; set; }
			public Guid OrderId { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<PurchaseDto>>
		{
			private readonly AppDataContext _context;
			private readonly IMapper _mapper;

			public Handler(AppDataContext context, IMapper mapper)
			{
				_mapper = mapper;
				_context = context;
			}

			public async Task<Result<PurchaseDto>> Handle(
				Command request,
				CancellationToken cancellationToken
			)
			{
					var purchaseToUpdate = _context.Purchases
						.Find(request.OrderId, request.PurchaseCreateParam.ProductId);

					if (purchaseToUpdate == null)
						return Result<PurchaseDto>.Failure("Purchase does not exist");
				
				try
				{

					purchaseToUpdate.QuantityPurchased = request.PurchaseCreateParam.QuantityPurchased;

					_context.Update(purchaseToUpdate);
					var success = await _context.SaveChangesAsync(cancellationToken) > 0;

					var purchaseDto = _mapper.Map<PurchaseDto>(purchaseToUpdate);

					if (success)
						return Result<PurchaseDto>.Success(purchaseDto);

					return Result<PurchaseDto>.Failure("Purchase update failed");
				}
				catch (Exception ex)
				{
					return Result<PurchaseDto>.Failure(ex.Message);
				}
			}
		}
	}
}
