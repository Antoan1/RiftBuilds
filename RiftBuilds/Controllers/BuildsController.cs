using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Builds;

namespace RiftBuilds.Controllers
{
    public class BuildsController : Controller
    {
        private readonly IBuildService buildService;

        public BuildsController(IBuildService buildService)
        {
            this.buildService = buildService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var builds = await buildService.GetAllAsync();

            return View(builds);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await buildService.GetDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new BuildFormViewModel();

            await buildService.PopulateOptionsAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuildFormViewModel model)
        {
            if (!await buildService.ChampionExistsAsync(model.ChampionId))
            {
                ModelState.AddModelError(
                    nameof(model.ChampionId),
                    "The selected champion does not exist.");
            }

            var itemIds = model.SelectedItemIds ?? Array.Empty<int>();

            if (itemIds.Distinct().Count() != itemIds.Length)
            {
                ModelState.AddModelError(
                    nameof(model.SelectedItemIds),
                    "You cannot select the same item more than once.");
            }

            if (!await buildService.ItemsExistAsync(itemIds))
            {
                ModelState.AddModelError(
                    nameof(model.SelectedItemIds),
                    "One or more selected items do not exist.");
            }

            if (!ModelState.IsValid)
            {
                await buildService.PopulateOptionsAsync(model);

                return View(model);
            }

            await buildService.CreateAsync(model);

            TempData["SuccessMessage"] = "Build created successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}