using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Builds;

namespace RiftBuilds.Controllers
{
    [Authorize]
    public class BuildsController : Controller
    {
        private readonly IBuildService buildService;

        public BuildsController(IBuildService buildService)
        {
            this.buildService = buildService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var builds = await buildService.GetAllAsync();

            return View(builds);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Mine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var builds = await buildService.GetMineAsync(userId);

            return View(builds);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var model = await buildService.GetDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["CanManage"] =
                User.Identity?.IsAuthenticated == true
                && !string.IsNullOrWhiteSpace(userId)
                && await buildService.CanManageAsync(
                    id,
                    userId,
                    User.IsInRole("Administrator"));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (!await buildService.CanManageAsync(
                id,
                userId,
                User.IsInRole("Administrator")))
            {
                return NotFound();
            }

            var model = await buildService.GetDetailsAsync(id);

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var deleted = await buildService.DeleteAsync(
                id,
                userId,
                User.IsInRole("Administrator"));

            if (!deleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Build deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var model = await buildService.GetForEditAsync(
                id,
                userId,
                User.IsInRole("Administrator"));

            if (model == null)
            {
                return NotFound();
            }

            ViewData["BuildId"] = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            BuildFormViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var isAdministrator = User.IsInRole("Administrator");

            if (!await buildService.CanManageAsync(
                id,
                userId,
                isAdministrator))
            {
                return NotFound();
            }

            await ValidateBuildSelectionAsync(model);

            if (!ModelState.IsValid)
            {
                await buildService.PopulateOptionsAsync(model);
                ViewData["BuildId"] = id;

                return View(model);
            }

            var updated = await buildService.UpdateAsync(
                id,
                model,
                userId,
                isAdministrator);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Build updated successfully.";

            return RedirectToAction(nameof(Details), new { id });
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            await ValidateBuildSelectionAsync(model);

            if (!ModelState.IsValid)
            {
                await buildService.PopulateOptionsAsync(model);

                return View(model);
            }

            await buildService.CreateAsync(model, userId);

            TempData["SuccessMessage"] = "Build created successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateBuildSelectionAsync(
            BuildFormViewModel model)
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
        }
    }
}