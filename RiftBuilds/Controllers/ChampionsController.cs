using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Champions;

namespace RiftBuilds.Controllers
{
    public class ChampionsController : Controller
    {
        private readonly IChampionService championService;

        public ChampionsController(IChampionService championService)
        {
            this.championService = championService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            [Bind("SearchTerm,CurrentPage")]
            ChampionQueryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid search or page parameters.");
            }

            await championService.GetFilteredAsync(model);

            return View(model);
        }
    }
}