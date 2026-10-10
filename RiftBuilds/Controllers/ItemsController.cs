using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Services.Interfaces;

namespace RiftBuilds.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemService itemService;

        public ItemsController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await itemService.GetAllAsync();

            return View(items);
        }
    }
}