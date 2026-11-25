using Microsoft.EntityFrameworkCore;
using RiftBuilds.Models;
using RiftBuilds.Models.Enums;
using RiftBuilds.Services;
using RiftBuilds.Tests.Infrastructure;
using RiftBuilds.ViewModels.Items;

namespace RiftBuilds.Tests.Services
{
    [TestFixture]
    public class ItemServiceTests
    {
        private TestDatabase database = null!;
        private ItemService service = null!;

        [SetUp]
        public void SetUp()
        {
            database = new TestDatabase();
            service = new ItemService(database.Context);
        }

        [TearDown]
        public void TearDown()
        {
            database.Dispose();
        }

        [Test]
        public async Task CreateAsync_SavesItemWithTrimmedValues()
        {
            var model = new ItemFormViewModel
            {
                Name = "  Test Item  ",
                Description = "  Test description.  ",
                Price = 1000,
                ImageUrl = "  /images/items/test.png  "
            };

            var id = await service.CreateAsync(model);

            database.Context.ChangeTracker.Clear();

            var savedItem = await database.Context.Items
                .SingleAsync(item => item.Id == id);

            Assert.Multiple(() =>
            {
                Assert.That(id, Is.GreaterThan(0));
                Assert.That(savedItem.Name, Is.EqualTo("Test Item"));
                Assert.That(
                    savedItem.Description,
                    Is.EqualTo("Test description."));
                Assert.That(savedItem.Price, Is.EqualTo(1000));
                Assert.That(
                    savedItem.ImageUrl,
                    Is.EqualTo("/images/items/test.png"));
            });
        }

        [Test]
        public async Task UpdateAsync_UpdatesExistingItem()
        {
            var item = await AddItemAsync();

            var model = new ItemFormViewModel
            {
                Name = "  Updated Item  ",
                Description = "  Updated description.  ",
                Price = 1500,
                ImageUrl = "  /images/items/updated.png  "
            };

            var result = await service.UpdateAsync(item.Id, model);

            database.Context.ChangeTracker.Clear();

            var updatedItem = await database.Context.Items
                .SingleAsync(i => i.Id == item.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(updatedItem.Name, Is.EqualTo("Updated Item"));
                Assert.That(
                    updatedItem.Description,
                    Is.EqualTo("Updated description."));
                Assert.That(updatedItem.Price, Is.EqualTo(1500));
                Assert.That(
                    updatedItem.ImageUrl,
                    Is.EqualTo("/images/items/updated.png"));
            });
        }

        [Test]
        public async Task DeleteAsync_RemovesItemWithoutBuilds()
        {
            var item = await AddItemAsync();

            var result = await service.DeleteAsync(item.Id);

            database.Context.ChangeTracker.Clear();

            var stillExists = await database.Context.Items
                .AnyAsync(i => i.Id == item.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(ItemDeleteResult.Success));
                Assert.That(stillExists, Is.False);
            });
        }

        [Test]
        public async Task DeleteAsync_PreservesItemUsedInBuild()
        {
            var item = await AddItemAsync();

            var champion = new Champion
            {
                Name = "Item Test Champion",
                Description = "Champion for item deletion tests.",
                ImageUrl = "/images/champions/test.jpg"
            };

            var build = new Build
            {
                Title = "Item test build",
                Champion = champion,
                Position = Position.Top
            };

            build.BuildItems.Add(new BuildItem
            {
                ItemId = item.Id,
                Slot = 1
            });

            database.Context.Builds.Add(build);
            await database.Context.SaveChangesAsync();

            database.Context.ChangeTracker.Clear();

            var result = await service.DeleteAsync(item.Id);

            database.Context.ChangeTracker.Clear();

            var itemExists = await database.Context.Items
                .AnyAsync(i => i.Id == item.Id);

            var buildExists = await database.Context.Builds
                .AnyAsync(b => b.Id == build.Id);

            var linkExists = await database.Context.BuildItems
                .AnyAsync(bi =>
                    bi.BuildId == build.Id && bi.ItemId == item.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(ItemDeleteResult.InUse));
                Assert.That(itemExists, Is.True);
                Assert.That(buildExists, Is.True);
                Assert.That(linkExists, Is.True);
            });
        }

        [Test]
        public async Task DeleteAsync_ReturnsNotFoundForMissingItem()
        {
            var result = await service.DeleteAsync(-1);

            Assert.That(result, Is.EqualTo(ItemDeleteResult.NotFound));
        }

        [TestCase(null, null, 4)]
        [TestCase(1000, null, 3)]
        [TestCase(null, 2000, 3)]
        [TestCase(1000, 2000, 2)]
        public async Task GetFilteredAsync_AppliesPriceLimits(
            int? minPrice,
            int? maxPrice,
            int expectedCount)
        {
            var prices = new[] { 0, 1000, 2000, 3000 };

            foreach (var price in prices)
            {
                database.Context.Items.Add(new Item
                {
                    Name = $"Price Filter Test {price:D4}",
                    Description = "Item for price filtering tests.",
                    Price = price,
                    ImageUrl = "/images/items/test.png"
                });
            }

            await database.Context.SaveChangesAsync();

            var model = new ItemQueryViewModel
            {
                SearchTerm = "Price Filter Test",
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            await service.GetFilteredAsync(model);

            var expectedPrices = prices
                .Where(price =>
                    (!minPrice.HasValue || price >= minPrice.Value)
                    && (!maxPrice.HasValue || price <= maxPrice.Value))
                .ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(model.TotalItems, Is.EqualTo(expectedCount));
                Assert.That(model.Items.Count(), Is.EqualTo(expectedCount));

                Assert.That(
                    model.Items.Select(item => item.Price),
                    Is.EqualTo(expectedPrices));
            });
        }

        private async Task<Item> AddItemAsync()
        {
            var item = new Item
            {
                Name = "Test Item",
                Description = "Test description.",
                Price = 1000,
                ImageUrl = "/images/items/test.png"
            };

            database.Context.Items.Add(item);
            await database.Context.SaveChangesAsync();

            return item;
        }
    }
}