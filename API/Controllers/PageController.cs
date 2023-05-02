using Application.Page;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PageController : BaseApiController
    {
        [HttpGet("Search")]
        public async Task<IActionResult> GetPage([FromQuery] GetPageParam getPageParam)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Page.GetPages.Query { GetPageParam = getPageParam }
                )
            );
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePage(CreatePageParam createPageParam)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Page.Create.Command { CreatePageParam = createPageParam }
                )
            );
        }

        [HttpPost("AddHeroPhoto")]
        public async Task<IActionResult> AddHeroPhoto(IFormFile heroPhoto, Guid pageId)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Page.AddHeroPhoto.Command
                    {
                        HeroPhoto = heroPhoto,
                        PageId = pageId
                    }
                )
            );
        }

        [HttpPost("AddLogoPhoto")]
        public async Task<IActionResult> AddlogoPhoto(IFormFile logoPhoto, Guid pageId)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Page.AddLogoPhoto.Command
                    {
                        LogoPhoto = logoPhoto,
                        PageId = pageId
                    }
                )
            );
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePage(PageUpdateParam pageUpdateParam)
        {
            return HandleResult(
                await Mediator.Send(
                    new Application.Page.Update.Command { PageUpdateParam = pageUpdateParam }
                )
            );
        }
    }
}
