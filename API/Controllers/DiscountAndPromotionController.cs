using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DiscountAndPromotionController : BaseApiController
    {
        [HttpPost("CreateNewDiscount")]
        public async Task<IActionResult> SubmitCardDetails(
            Application.DiscountAndPromotions.CreateParam createDiscountParam
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.DiscountAndPromotions.Create.Command
                    {
                        CreateParam = createDiscountParam
                    }
                )
            );
        }

        [HttpPut("UpdateDiscount")]
        public async Task<IActionResult> UpdateDiscount(
            Application.DiscountAndPromotions.CreateParam createDiscountParam,
            Guid discountId
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.DiscountAndPromotions.UpDate.Command
                    {
                        CreateParam = createDiscountParam,
                        DiscountId = discountId
                    }
                )
            );
        }

        [HttpPost("ApplyToProduct")]
        public async Task<IActionResult> ApplyToProduct(
            List<Guid> listOfproductIds,
            Guid discountId
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.DiscountAndPromotions.ApplyToProduct.Command
                    {
                        ProductIds = listOfproductIds,
                        DiscountId = discountId
                    }
                )
            );
        }

        [HttpPost("RemoveFromProduct")]
        public async Task<IActionResult> RemoveFromProduct(
            List<Guid> listOfproductIds,
            Guid discountId
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.DiscountAndPromotions.RemoveFromProduct.Command
                    {
                        ProductIds = listOfproductIds,
                        DiscountId = discountId
                    }
                )
            );
        }

        [HttpDelete("DeleteDiscount")]
        public async Task<IActionResult> DeleteDiscount(Guid discountId)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.DiscountAndPromotions.Delete.Command { DiscountId = discountId }
                )
            );
        }

        [HttpGet("GetAllDiscounts")]
        public async Task<IActionResult> GetAll(Guid storeId)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.DiscountAndPromotions.GetDiscounts.Command { StoreId = storeId }
                )
            );
        }
    }
}
