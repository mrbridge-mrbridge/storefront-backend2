using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Product
{
    public class Update
    {
        public class Command : IRequest<Result<ProductDto>>
        {
            public ProductUpdateParam ProductUpdateParam { get; set; }
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
                var ProductToUpdate = _context.Products
                    .Include(p => p.Store)
                    .FirstOrDefault(p => p.ProductId == request.ProductUpdateParam.ProductId);

                if (ProductToUpdate == null)
                    return Result<ProductDto>.Failure("Product does not exist");

                try
                {
                    ProductToUpdate.ProductName = request.ProductUpdateParam.ProductName;
                    ProductToUpdate.ProductDescription = request
                        .ProductUpdateParam
                        .ProductDescription;
                    ProductToUpdate.ProductCategory = request.ProductUpdateParam.ProductCategory;
                    ProductToUpdate.UnitOfMeasurement = request
                        .ProductUpdateParam
                        .UnitOfMeasurement;
                    ProductToUpdate.Quantity = request.ProductUpdateParam.Quantity;
                    ProductToUpdate.UnitPrice = request.ProductUpdateParam.UnitPrice;

                    _context.Products.Update(ProductToUpdate);
                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var ProductToReturn = _mapper.Map<ProductDto>(ProductToUpdate);

                        return Result<ProductDto>.Success(ProductToReturn);
                    }
                    return Result<ProductDto>.Failure("Couldn't update product");
                }
                catch (Exception ex)
                {
                    return Result<ProductDto>.Failure(ex.Message);
                }
            }
        }
    }
}
