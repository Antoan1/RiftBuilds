using Microsoft.EntityFrameworkCore;
using RiftBuilds.Models;
using RiftBuilds.Models.Enums;
using RiftBuilds.Services;
using RiftBuilds.Tests.Infrastructure;
using RiftBuilds.ViewModels.Champions;

namespace RiftBuilds.Tests.Services
{
    [TestFixture]
    public class ChampionServiceTests
    {
        private TestDatabase database = null!;
        private ChampionService service = null!;

        [SetUp]
        public void SetUp()
        {
            database = new TestDatabase();
            service = new ChampionService(database.Context);
        }

        [TearDown]
        public void TearDown()
        {
            database.Dispose();
        }

        [Test]
        public async Task CreateAsync_SavesChampionWithTrimmedValues()
        {
            var model = new ChampionFormViewModel
            {
                Name = "  Test Champion  ",
                Description = "  Test description.  ",
                ImageUrl = "  /images/champions/test.jpg  "
            };

            var id = await service.CreateAsync(model);

            database.Context.ChangeTracker.Clear();

            var savedChampion = await database.Context.Champions
                .SingleAsync(champion => champion.Id == id);

            Assert.Multiple(() =>
            {
                Assert.That(id, Is.GreaterThan(0));
                Assert.That(savedChampion.Name, Is.EqualTo("Test Champion"));
                Assert.That(
                    savedChampion.Description,
                    Is.EqualTo("Test description."));
                Assert.That(
                    savedChampion.ImageUrl,
                    Is.EqualTo("/images/champions/test.jpg"));
            });
        }

        [Test]
        public async Task UpdateAsync_UpdatesExistingChampion()
        {
            var champion = await AddChampionAsync();

            var model = new ChampionFormViewModel
            {
                Name = "  Updated Champion  ",
                Description = "  Updated description.  ",
                ImageUrl = "  /images/champions/updated.jpg  "
            };

            var result = await service.UpdateAsync(champion.Id, model);

            database.Context.ChangeTracker.Clear();

            var updatedChampion = await database.Context.Champions
                .SingleAsync(c => c.Id == champion.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(
                    updatedChampion.Name,
                    Is.EqualTo("Updated Champion"));
                Assert.That(
                    updatedChampion.Description,
                    Is.EqualTo("Updated description."));
                Assert.That(
                    updatedChampion.ImageUrl,
                    Is.EqualTo("/images/champions/updated.jpg"));
            });
        }

        [Test]
        public async Task UpdateAsync_ReturnsFalseForMissingChampion()
        {
            var model = new ChampionFormViewModel
            {
                Name = "Missing Champion",
                Description = "Test description.",
                ImageUrl = "/images/champions/test.jpg"
            };

            var result = await service.UpdateAsync(-1, model);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_RemovesChampionWithoutBuilds()
        {
            var champion = await AddChampionAsync();

            var result = await service.DeleteAsync(champion.Id);

            database.Context.ChangeTracker.Clear();

            var stillExists = await database.Context.Champions
                .AnyAsync(c => c.Id == champion.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(ChampionDeleteResult.Success));
                Assert.That(stillExists, Is.False);
            });
        }

        [Test]
        public async Task DeleteAsync_PreservesChampionUsedInBuild()
        {
            var champion = await AddChampionAsync();

            var build = new Build
            {
                Title = "Test build",
                ChampionId = champion.Id,
                Position = Position.Top
            };

            database.Context.Builds.Add(build);
            await database.Context.SaveChangesAsync();

            database.Context.ChangeTracker.Clear();

            var result = await service.DeleteAsync(champion.Id);

            database.Context.ChangeTracker.Clear();

            var championExists = await database.Context.Champions
                .AnyAsync(c => c.Id == champion.Id);

            var buildExists = await database.Context.Builds
                .AnyAsync(b => b.Id == build.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(ChampionDeleteResult.InUse));
                Assert.That(championExists, Is.True);
                Assert.That(buildExists, Is.True);
            });
        }

        [Test]
        public async Task DeleteAsync_ReturnsNotFoundForMissingChampion()
        {
            var result = await service.DeleteAsync(-1);

            Assert.That(
                result,
                Is.EqualTo(ChampionDeleteResult.NotFound));
        }

        private async Task<Champion> AddChampionAsync()
        {
            var champion = new Champion
            {
                Name = "Test Champion",
                Description = "Test description.",
                ImageUrl = "/images/champions/test.jpg"
            };

            database.Context.Champions.Add(champion);
            await database.Context.SaveChangesAsync();

            return champion;
        }
    }
}