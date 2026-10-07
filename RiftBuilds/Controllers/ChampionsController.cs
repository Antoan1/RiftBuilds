using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Services.Interfaces;

namespace RiftBuilds.Controllers
{
    public class ChampionsController : Controller
    {
        private readonly IChampionService championService;

        public ChampionsController(IChampionService championService)
        {
            this.championService = championService;
        }

        public async Task<IActionResult> Index()
        {
            var champions = await championService.GetAllAsync();

            return View(champions);
        }
    }
}