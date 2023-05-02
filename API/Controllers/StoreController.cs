using Application.Page;
using Application.Store;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StoreController : BaseApiController
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateStore(CreateHomePageParam param)
        {
            StoreDto store = (
                await Mediator.Send(
                    new Application.Store.Create.Command { CreateHomePageParam = param }
                )
            ).Value;

            param.CreatePageParam.StoreId = store.StoreId;

            PageDto page = (
                await Mediator.Send(
                    new Application.Page.Create.Command { CreatePageParam = param.CreatePageParam }
                )
            ).Value;

            return Ok(new CreateHomePageDto { HomePage = page, Store = store });
        }

        [HttpGet("GetStore")]
        public async Task<IActionResult> GetStore(Guid merchantId, Guid storeId)
        {
            var storeList = await Mediator.Send(
                new GetStore.Query { MerchantId = merchantId, StoreId = storeId }
            );

            if (!storeList.IsSuccess)
                return HandleResult(storeList);

            if (storeId != Guid.Empty)
                return Ok(
                    storeList.Value.First() != null ? storeList.Value.First() : storeList.Value
                );

            return HandleResult(storeList);
        }
    }
}
