using Application.Product;
using Application.ReviewReplies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchProducts(
            string productCategory,
            string productName
        )
        {
            return HandleResult(
                await _mediator.Send(
                    new Products.Query
                    {
                        StoreId = Guid.Empty,
                        ProductCategory = productCategory,
                        ProductName = productName
                    }
                )
            );
        }

        [HttpGet("GetStoreProducts")]
        public async Task<IActionResult> GetStoreProducts(
            Guid storeId,
            string category
        )
        {
            return HandleResult(
                await _mediator.Send(
                    new Products.Query
                    {
                        StoreId = storeId,
                        ProductCategory = category,
                        ProductName = string.Empty
                    }
                )
            );
        }
        [HttpGet("GetStoreCategories")]
        public async Task<IActionResult> GetStoreCategories(
            Guid storeId
        )
        {
            return HandleResult(
                await _mediator.Send(
                    new GetStoreProductCategories.Query
                    {
                        StoreId = storeId,
                    }
                )
            );
        }

        [HttpGet("GetProduct")]
        public async Task<IActionResult> CreateProduct(Guid productId)
        {
            var result = await _mediator.Send(new Details.Query { ProductId = productId });
            if(!result.IsSuccess)
                return HandleResult(result);

            return HandleResult(
                await _mediator.Send(new GetReply.Query { ProductDto = result.Value })
            );

        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(ProductCreateParam product)
        {
            return HandleResult(
                await _mediator.Send(new Create.Command { ProductCreateParam = product })
            );
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct(ProductUpdateParam productUpdateParam)
        {
            return HandleResult(
                await _mediator.Send(new Update.Command { ProductUpdateParam = productUpdateParam })
            );
        }

        [HttpPost("AddPhoto")]
        public async Task<IActionResult> UpdateProduct(IFormFile productPhoto, Guid productId)
        {
            return HandleResult(
                await _mediator.Send(
                    new AddPhoto.Command { ProductPhoto = productPhoto, ProductId = productId }
                )
            );
        }
    }
}
