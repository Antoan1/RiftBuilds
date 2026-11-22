using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiftBuilds.Models.Enums;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Items;

namespace RiftBuilds.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ItemsController : Controller
    {
        private readonly IItemService itemService;

        public ItemsController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await itemService.GetAllAsync();

            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ItemFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await itemService.NameExistsAsync(model.Name))
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "An item with this name already exists.");

                return View(model);
            }

            await itemService.CreateAsync(model);

            TempData["SuccessMessage"] = "Item created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await itemService.GetForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["ItemId"] = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ItemFormViewModel model)
        {
            if (await itemService.GetForEditAsync(id) == null)
            {
                return NotFound();
            }

            ViewData["ItemId"] = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await itemService.NameExistsAsync(model.Name, id))
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "An item with this name already exists.");

                return View(model);
            }

            var updated = await itemService.UpdateAsync(id, model);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Item updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await itemService.GetForDeleteAsync(id);

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
            var result = await itemService.DeleteAsync(id);

            if (result == ItemDeleteResult.NotFound)
            {
                return NotFound();
            }

            if (result == ItemDeleteResult.InUse)
            {
                var model = await itemService.GetForDeleteAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    "This item cannot be deleted because it is used in a build.");

                return View("Delete", model);
            }

            TempData["SuccessMessage"] = "Item deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}