using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Items;

namespace RiftBuilds.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemService itemService;

        public ItemsController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            [Bind("SearchTerm,MinPrice,MaxPrice,CurrentPage")]
            ItemQueryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await itemService.GetFilteredAsync(model);

            return View(model);
        }
    }
}