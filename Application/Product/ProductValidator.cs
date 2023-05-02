using FluentValidation;

namespace Application.Product
{
    public class ProductValidator: AbstractValidator<ProductCreateParam>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.ProductDescription).NotEmpty();
            RuleFor(x => x.ProductCategory).NotEmpty();
            RuleFor(x => x.UnitOfMeasurement).NotNull();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.StoreId).NotNull();
        }
    }
}
