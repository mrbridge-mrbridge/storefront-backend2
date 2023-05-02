using System.Linq;
using Application.DiscountAndPromotions;
using Application.Orders;
using Application.Purchases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
    {
        [HttpPost("AddToCart")]
        public async Task<IActionResult> PuchaseAProduct(PurchaseCreateParam purchaseCreateParam)
        {
            var orderId = await Mediator.Send(
                new Application.Orders.Create.Command
                {
                    OrderCreateParam = new OrderCreateParam(purchaseCreateParam.CustomerId)
                }
            );

            if (!orderId.IsSuccess)
                return HandleResult(orderId);

            var success = (
                await Mediator.Send(
                    new Application.Purchases.Create.Command
                    {
                        PurchaseCreateParam = purchaseCreateParam,
                        OrderId = orderId.Value
                    }
                )
            );

            if (!success.IsSuccess)
                return HandleResult(success);

            return HandleResult(
                await Mediator.Send(
                    new CalculateOnPurchase.Command
                    {
                        OrderId = orderId.Value,
                        ProductId = purchaseCreateParam.ProductId
                    }
                )
            );
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdatePurchase(
            PurchaseUpdateParam purchaseUpdateParam,
            Guid orderId
        )
        {
            return HandleResult(
                await Mediator.Send(
                    new Update.Command
                    {
                        PurchaseCreateParam = purchaseUpdateParam,
                        OrderId = orderId
                    }
                )
            );
        }

        [HttpDelete("RemoveFromCart")]
        public async Task<IActionResult> DeletePurchase(Guid productId, Guid orderId)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Purchases.Delete.Command
                    {
                        ProductId = productId,
                        OrderId = orderId
                    }
                )
            );
        }

        [HttpGet("CustomerCart")]
        public async Task<IActionResult> Cart(Guid customerId)
        {
            var dto = await Mediator.Send(new Cart.Query { CustomerId = customerId, Cart = true });

            var withTotals = await Mediator.Send(new CalculateTotals.Query { OrderDto = dto.Value });

            return Ok(withTotals.Value.FirstOrDefault());
        }

        [HttpGet("CustomerOrders")]
        public async Task<IActionResult> CustomerOrder(Guid customerId)
        {
            var dto = await Mediator.Send(new Cart.Query { CustomerId = customerId, Cart = false });

            return HandleResult(
                await Mediator.Send(new CalculateTotals.Query { OrderDto = dto.Value })
            );
        }

        [HttpGet("CustomerCartForThisStore")]
        public async Task<IActionResult> CustomerCartForStore(Guid customerId, Guid storeId)
        {
            var dto = await Mediator.Send(new CustomerOrdersForAStore.Query { CustomerId = customerId, StoreId = storeId , Cart = true });

            return Ok(
                (await Mediator.Send(new CalculateTotals.Query { OrderDto = dto.Value })).Value.FirstOrDefault()
            );
        }

        [HttpGet("CustomerOrdersForStore")]
        public async Task<IActionResult> CustomerOrderForStore(Guid customerId, Guid storeId)
        {
            var dto = await Mediator.Send(new CustomerOrdersForAStore.Query { CustomerId = customerId, StoreId = storeId , Cart = false });

            return HandleResult(
                await Mediator.Send(new CalculateTotals.Query { OrderDto = dto.Value })
            );
        }

        [HttpGet("GetStoreOrders")]
        public async Task<IActionResult> StoreOrders(Guid storeId)
        {
            return HandleResult(
                await Mediator.Send(new GetCustomerOrders.Query { StoreId = storeId, Cart = false })
            );
        }

        [HttpGet("GetStoreCart")]
        public async Task<IActionResult> StoreCart(Guid storeId)
        {
            return Ok(
                (await Mediator.Send(new GetCustomerOrders.Query { StoreId = storeId, Cart = true })).Value.FirstOrDefault()
            );
        }

        [HttpPut("TagOrderAsProcessing")]
        public async Task<IActionResult> TagAsProcess(Guid orderId)
        {
            return HandleResult(
                await Mediator.Send(new TagProcessing.Command { OrderId = orderId })
            );
        }

        [HttpPut("TagOrderAsShipping")]
        public async Task<IActionResult> TagAsShipping(Guid orderId)
        {
            return HandleResult(await Mediator.Send(new TagShipping.Command { OrderId = orderId }));
        }

        [HttpPut("TagOrderDelivered")]
        public async Task<IActionResult> TagAsDelivered(Guid orderId)
        {
            return HandleResult(
                await Mediator.Send(new TagDelivered.Command { OrderId = orderId })
            );
        }

        [HttpPut("TagOrderAsCancelled")]
        public async Task<IActionResult> TagAsCancelled(Guid orderId)
        {
            var cancellation = await Mediator.Send(new TagCancelled.Command { OrderId = orderId });

            if (cancellation.IsSuccess)
            {
                foreach (OrderCancellationDto cancellationDto in cancellation.Value)
                {
                    var success = await Mediator.Send(
                        new Application.Purchases.Delete.Command
                        {
                            ProductId = cancellationDto.ProductId,
                            OrderId = cancellationDto.OrderId
                        }
                    );

                    if (!success.IsSuccess)
                        return HandleResult(success);
                }
            }

            return HandleResult(cancellation);
        }
    }
}
