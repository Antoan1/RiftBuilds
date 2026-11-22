using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Models.Enums;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Champions;

namespace RiftBuilds.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ChampionsController : Controller
    {
        private readonly IChampionService championService;

        public ChampionsController(IChampionService championService)
        {
            this.championService = championService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var champions = await championService.GetAllAsync();

            return View(champions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ChampionFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ChampionFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await championService.NameExistsAsync(model.Name))
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "A champion with this name already exists.");

                return View(model);
            }

            await championService.CreateAsync(model);

            TempData["SuccessMessage"] = "Champion created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await championService.GetForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["ChampionId"] = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ChampionFormViewModel model)
        {
            if (await championService.GetForEditAsync(id) == null)
            {
                return NotFound();
            }

            ViewData["ChampionId"] = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await championService.NameExistsAsync(model.Name, id))
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "A champion with this name already exists.");

                return View(model);
            }

            var updated = await championService.UpdateAsync(id, model);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Champion updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await championService.GetForDeleteAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await championService.DeleteAsync(id);

            if (result == ChampionDeleteResult.NotFound)
            {
                return NotFound();
            }

            if (result == ChampionDeleteResult.InUse)
            {
                var model = await championService.GetForDeleteAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    "This champion cannot be deleted because it is used in a build.");

                return View("Delete", model);
            }

            TempData["SuccessMessage"] = "Champion deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}