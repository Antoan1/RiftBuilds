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

        [Test]
        public async Task NameExistsAsync_IgnoresCaseAndSurroundingSpaces()
        {
            await AddChampionAsync();

            var result = await service.NameExistsAsync("  test CHAMPION  ");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task NameExistsAsync_ExcludesCurrentChampion()
        {
            var champion = await AddChampionAsync();

            var result = await service.NameExistsAsync(
                champion.Name,
                champion.Id);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task NameExistsAsync_FindsAnotherChampionDespiteExcludedId()
        {
            var champion = await AddChampionAsync();

            var otherChampion = new Champion
            {
                Name = "Another Test Champion",
                Description = "Another description.",
                ImageUrl = "/images/champions/another.jpg"
            };

            database.Context.Champions.Add(otherChampion);
            await database.Context.SaveChangesAsync();

            var result = await service.NameExistsAsync(
                champion.Name,
                otherChampion.Id);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task NameExistsAsync_ReturnsFalseForUnknownName()
        {
            var result = await service.NameExistsAsync(
                "Nonexistent Champion 987654");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsExistingChampionValues()
        {
            var champion = await AddChampionAsync();

            var model = await service.GetForEditAsync(champion.Id);

            Assert.That(model, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(model!.Name, Is.EqualTo(champion.Name));
                Assert.That(model.Description, Is.EqualTo(champion.Description));
                Assert.That(model.ImageUrl, Is.EqualTo(champion.ImageUrl));
            });
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNullForMissingChampion()
        {
            var model = await service.GetForEditAsync(-1);

            Assert.That(model, Is.Null);
        }

        [Test]
        public async Task GetFilteredAsync_FindsChampionByPartOfName()
        {
            var champion = await AddChampionAsync();

            var model = new ChampionQueryViewModel
            {
                SearchTerm = "  est Champ  "
            };

            await service.GetFilteredAsync(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.SearchTerm, Is.EqualTo("est Champ"));
                Assert.That(model.TotalChampions, Is.EqualTo(1));
                Assert.That(
                    model.Champions.Select(c => c.Id),
                    Is.EqualTo(new[] { champion.Id }));
            });
        }

        [Test]
        public async Task GetFilteredAsync_ReturnsEmptyResultsWhenNothingMatches()
        {
            var model = new ChampionQueryViewModel
            {
                SearchTerm = "Nonexistent Champion 987654",
                CurrentPage = 5
            };

            await service.GetFilteredAsync(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.Champions, Is.Empty);
                Assert.That(model.TotalChampions, Is.Zero);
                Assert.That(model.TotalPages, Is.Zero);
                Assert.That(model.CurrentPage, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task GetFilteredAsync_ReturnsCorrectPagesInNameOrder()
        {
            const string prefix = "Pagination Test ";

            for (int i = 11; i >= 1; i--)
            {
                database.Context.Champions.Add(new Champion
                {
                    Name = $"{prefix}{i:D2}",
                    Description = "Pagination test champion.",
                    ImageUrl = "/images/champions/test.jpg"
                });
            }

            await database.Context.SaveChangesAsync();

            var firstPage = new ChampionQueryViewModel
            {
                SearchTerm = prefix,
                CurrentPage = 1
            };

            var secondPage = new ChampionQueryViewModel
            {
                SearchTerm = prefix,
                CurrentPage = 2
            };

            await service.GetFilteredAsync(firstPage);
            await service.GetFilteredAsync(secondPage);

            var expectedFirstPage = Enumerable.Range(1, 9)
                .Select(i => $"{prefix}{i:D2}")
                .ToArray();

            var expectedSecondPage = new[]
            {
        $"{prefix}10",
        $"{prefix}11"
    };

            Assert.Multiple(() =>
            {
                Assert.That(firstPage.TotalChampions, Is.EqualTo(11));
                Assert.That(firstPage.TotalPages, Is.EqualTo(2));
                Assert.That(firstPage.CurrentPage, Is.EqualTo(1));

                Assert.That(
                    firstPage.Champions.Select(c => c.Name),
                    Is.EqualTo(expectedFirstPage));

                Assert.That(secondPage.TotalChampions, Is.EqualTo(11));
                Assert.That(secondPage.CurrentPage, Is.EqualTo(2));

                Assert.That(
                    secondPage.Champions.Select(c => c.Name),
                    Is.EqualTo(expectedSecondPage));
            });
        }

        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(100)]
        public async Task GetFilteredAsync_AdjustsPageOutsideAvailableRange(
            int requestedPage)
        {
            var champion = await AddChampionAsync();

            var model = new ChampionQueryViewModel
            {
                SearchTerm = champion.Name,
                CurrentPage = requestedPage
            };

            await service.GetFilteredAsync(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.CurrentPage, Is.EqualTo(1));
                Assert.That(model.TotalChampions, Is.EqualTo(1));
                Assert.That(
                    model.Champions.Select(c => c.Id),
                    Is.EqualTo(new[] { champion.Id }));
            });
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