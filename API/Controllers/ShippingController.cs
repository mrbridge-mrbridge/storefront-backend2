using Application.Orders;
using Application.Purchases;
using Application.Shipping;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ShippingController : BaseApiController
    {
        [HttpPost("AddShippingDetails")]
        public async Task<IActionResult> AddShippingDetails(ShippingParam shippingParam)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Shipping.Create.Command { ShippingParam = shippingParam }
                )
            );
        }

        [HttpPut("UpdateShippingDetail")]
        public async Task<IActionResult> UpdateShippingDetail(
            UpdateParam updateParam,
            Guid shippingId
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Shipping.Update.Command
                    {
                        UpdateParam = updateParam,
                        ShippingDetailId = shippingId
                    }
                )
            );
        }

        [HttpDelete("DeleteShippingDetail")]
        public async Task<IActionResult> DeleteShippingDetail(Guid shippingId)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Shipping.Delete.Command { ShippingDetailId = shippingId }
                )
            );
        }

        [HttpGet("GetCustomerShippingDetail")]
        public async Task<IActionResult> GetCustomerShippingDetail(Guid CutomerId)
        {
            return HandleResult(
                await Mediator.Send(new GetShippingDetails.Query { CustomerId = CutomerId })
            );
        }

        [HttpGet("GetCustomerStoreShippingDetail")]
        public async Task<IActionResult> GetCustomerStoreShippingDetail(
            Guid CutomerId,
            Guid storeId
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new GetShippingDetails.Query { CustomerId = CutomerId, StoreId = storeId }
                )
            );
        }

        [HttpGet("GetShippingDetail")]
        public async Task<IActionResult> GetShippingDetail(Guid shippingDetailId)
        {
            return HandleResult(
                await Mediator.Send(new GetADetail.Query { ShippingDetailId = shippingDetailId })
            );
        }

        [HttpPut("ApplyToAnOrder")]
        public async Task<IActionResult> ApplyToAOrder(Guid orderId, Guid shippingDetailId)
        {
            return HandleResult(
                await Mediator.Send(
                    new ApplyToOrder.Command
                    {
                        ShippingDetailId = shippingDetailId,
                        OrderId = orderId
                    }
                )
            );
        }

        [HttpPut("RemoveFromOrder")]
        public async Task<IActionResult> RemoveFromOrder(Guid orderId, Guid shippingDetailId)
        {
            return HandleResult(
                await Mediator.Send(
                    new RemoveFromOrder.Command
                    {
                        ShippingDetailId = shippingDetailId,
                        OrderId = orderId
                    }
                )
            );
        }
    }
}
