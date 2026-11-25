using Microsoft.EntityFrameworkCore;
using RiftBuilds.Models;
using RiftBuilds.Models.Enums;
using RiftBuilds.Services;
using RiftBuilds.Tests.Infrastructure;
using RiftBuilds.ViewModels.Builds;

namespace RiftBuilds.Tests.Services
{
    [TestFixture]
    public class BuildServiceTests
    {
        private const string AuthorId = "test-author";
        private const string OtherUserId = "test-other-user";

        private TestDatabase database = null!;
        private BuildService service = null!;

        private int championId;
        private int itemId;

        [SetUp]
        public async Task SetUp()
        {
            database = new TestDatabase();
            service = new BuildService(database.Context);

            database.Context.Users.AddRange(
                new ApplicationUser
                {
                    Id = AuthorId,
                    UserName = "author@test.local",
                    NormalizedUserName = "AUTHOR@TEST.LOCAL",
                    Email = "author@test.local",
                    NormalizedEmail = "AUTHOR@TEST.LOCAL"
                },
                new ApplicationUser
                {
                    Id = OtherUserId,
                    UserName = "other@test.local",
                    NormalizedUserName = "OTHER@TEST.LOCAL",
                    Email = "other@test.local",
                    NormalizedEmail = "OTHER@TEST.LOCAL"
                });

            var champion = new Champion
            {
                Name = "Build Test Champion",
                Description = "Champion for build tests.",
                ImageUrl = "/images/champions/test.jpg"
            };

            var item = new Item
            {
                Name = "Build Test Item",
                Description = "Item for build tests.",
                Price = 1000,
                ImageUrl = "/images/items/test.png"
            };

            database.Context.Champions.Add(champion);
            database.Context.Items.Add(item);

            await database.Context.SaveChangesAsync();

            championId = champion.Id;
            itemId = item.Id;
        }

        [TearDown]
        public void TearDown()
        {
            database.Dispose();
        }

        [Test]
        public async Task CreateAsync_SavesAuthorAndSelectedItems()
        {
            var model = new BuildFormViewModel
            {
                Title = "  New Build  ",
                Description = null,
                ChampionId = championId,
                Position = Position.Top,
                SelectedItemIds = new[] { itemId }
            };

            var id = await service.CreateAsync(model, AuthorId);

            database.Context.ChangeTracker.Clear();

            var savedBuild = await database.Context.Builds
                .Include(build => build.BuildItems)
                .SingleAsync(build => build.Id == id);

            Assert.Multiple(() =>
            {
                Assert.That(savedBuild.AuthorId, Is.EqualTo(AuthorId));
                Assert.That(savedBuild.Title, Is.EqualTo("New Build"));
                Assert.That(savedBuild.Description, Is.Null);
                Assert.That(savedBuild.ChampionId, Is.EqualTo(championId));
                Assert.That(savedBuild.Position, Is.EqualTo(Position.Top));
                Assert.That(savedBuild.BuildItems.Count, Is.EqualTo(1));
            });

            var selectedItem = savedBuild.BuildItems.Single();

            Assert.Multiple(() =>
            {
                Assert.That(selectedItem.ItemId, Is.EqualTo(itemId));
                Assert.That(selectedItem.Slot, Is.EqualTo(1));
            });
        }

        [TestCase(AuthorId, false, true)]
        [TestCase(OtherUserId, false, false)]
        [TestCase(OtherUserId, true, true)]
        public async Task UpdateAsync_RespectsOwnershipAndAdministratorAccess(
            string currentUserId,
            bool isAdministrator,
            bool expectedSuccess)
        {
            var build = await AddBuildAsync(AuthorId, "Original Build");

            var model = new BuildFormViewModel
            {
                Title = "Updated Build",
                Description = "Updated description.",
                ChampionId = championId,
                Position = Position.Mid,
                SelectedItemIds = new[] { itemId }
            };

            database.Context.ChangeTracker.Clear();

            var result = await service.UpdateAsync(
                build.Id,
                model,
                currentUserId,
                isAdministrator);

            database.Context.ChangeTracker.Clear();

            var savedBuild = await database.Context.Builds
                .Include(b => b.BuildItems)
                .SingleAsync(b => b.Id == build.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expectedSuccess));

                Assert.That(
                    savedBuild.Title,
                    Is.EqualTo(
                        expectedSuccess ? "Updated Build" : "Original Build"));

                Assert.That(
                    savedBuild.Description,
                    Is.EqualTo(
                        expectedSuccess
                            ? "Updated description."
                            : "Original description."));

                Assert.That(
                    savedBuild.Position,
                    Is.EqualTo(
                        expectedSuccess ? Position.Mid : Position.Top));

                Assert.That(savedBuild.AuthorId, Is.EqualTo(AuthorId));

                Assert.That(
                    savedBuild.BuildItems.Select(bi => bi.ItemId),
                    Is.EqualTo(new[] { itemId }));
            });
        }

        [TestCase(AuthorId, false, true)]
        [TestCase(OtherUserId, false, false)]
        [TestCase(OtherUserId, true, true)]
        public async Task DeleteAsync_RespectsOwnershipAndAdministratorAccess(
            string currentUserId,
            bool isAdministrator,
            bool expectedSuccess)
        {
            var build = await AddBuildAsync(AuthorId, "Delete Test");

            database.Context.ChangeTracker.Clear();

            var result = await service.DeleteAsync(
                build.Id,
                currentUserId,
                isAdministrator);

            database.Context.ChangeTracker.Clear();

            var buildExists = await database.Context.Builds
                .AnyAsync(b => b.Id == build.Id);

            var linkExists = await database.Context.BuildItems
                .AnyAsync(bi => bi.BuildId == build.Id);

            var itemExists = await database.Context.Items
                .AnyAsync(item => item.Id == itemId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expectedSuccess));
                Assert.That(buildExists, Is.EqualTo(!expectedSuccess));
                Assert.That(linkExists, Is.EqualTo(!expectedSuccess));
                Assert.That(itemExists, Is.True);
            });
        }

        [Test]
        public async Task GetMineAsync_ReturnsOnlyCurrentUsersBuilds()
        {
            var ownBuild = await AddBuildAsync(
                AuthorId,
                "My Build");

            await AddBuildAsync(
                OtherUserId,
                "Another Users Build");

            var results = await service.GetMineAsync(AuthorId);

            Assert.That(
                results.Select(build => build.Id),
                Is.EqualTo(new[] { ownBuild.Id }));
        }

        [TestCase(AuthorId, false, true)]
        [TestCase(OtherUserId, false, false)]
        [TestCase(OtherUserId, true, true)]
        [TestCase("", true, false)]
        public async Task CanManageAsync_ChecksUserPermissions(
    string currentUserId,
    bool isAdministrator,
    bool expectedResult)
        {
            var build = await AddBuildAsync(AuthorId, "Permission Test");

            var result = await service.CanManageAsync(
                build.Id,
                currentUserId,
                isAdministrator);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task CanManageAsync_ReturnsFalseForMissingBuild()
        {
            var result = await service.CanManageAsync(
                -1,
                AuthorId,
                true);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsBuildAndItemDetails()
        {
            var build = await AddBuildAsync(AuthorId, "Details Test");

            var model = await service.GetDetailsAsync(build.Id);

            Assert.That(model, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(model!.Id, Is.EqualTo(build.Id));
                Assert.That(model.Title, Is.EqualTo("Details Test"));
                Assert.That(
                    model.Description,
                    Is.EqualTo("Original description."));
                Assert.That(
                    model.ChampionName,
                    Is.EqualTo("Build Test Champion"));
                Assert.That(
                    model.ChampionImageUrl,
                    Is.EqualTo("/images/champions/test.jpg"));
                Assert.That(model.Position, Is.EqualTo(Position.Top));
                Assert.That(model.CreatedOn, Is.EqualTo(build.CreatedOn));
                Assert.That(model.TotalPrice, Is.EqualTo(1000));
                Assert.That(model.Items.Count, Is.EqualTo(1));
            });

            var selectedItem = model!.Items.Single();

            Assert.Multiple(() =>
            {
                Assert.That(selectedItem.Name, Is.EqualTo("Build Test Item"));
                Assert.That(
                    selectedItem.ImageUrl,
                    Is.EqualTo("/images/items/test.png"));
                Assert.That(selectedItem.Price, Is.EqualTo(1000));
                Assert.That(selectedItem.Slot, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNullForMissingBuild()
        {
            var model = await service.GetDetailsAsync(-1);

            Assert.That(model, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsFormAndOptionsForAuthor()
        {
            var build = await AddBuildAsync(AuthorId, "Edit Form Test");

            var model = await service.GetForEditAsync(
                build.Id,
                AuthorId,
                false);

            Assert.That(model, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(model!.Title, Is.EqualTo("Edit Form Test"));
                Assert.That(
                    model.Description,
                    Is.EqualTo("Original description."));
                Assert.That(model.ChampionId, Is.EqualTo(championId));
                Assert.That(model.Position, Is.EqualTo(Position.Top));
                Assert.That(
                    model.SelectedItemIds,
                    Is.EqualTo(new[] { itemId }));

                Assert.That(
                    model.Champions.Any(c =>
                        c.Value == championId.ToString()
                        && c.Text == "Build Test Champion"),
                    Is.True);

                Assert.That(
                    model.Items.Any(i =>
                        i.Id == itemId
                        && i.Name == "Build Test Item"
                        && i.ImageUrl == "/images/items/test.png"),
                    Is.True);
            });
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNullForAnotherUser()
        {
            var build = await AddBuildAsync(AuthorId, "Private Edit Test");

            var model = await service.GetForEditAsync(
                build.Id,
                OtherUserId,
                false);

            Assert.That(model, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_AllowsAdministrator()
        {
            var build = await AddBuildAsync(AuthorId, "Admin Edit Test");

            var model = await service.GetForEditAsync(
                build.Id,
                OtherUserId,
                true);

            Assert.That(model, Is.Not.Null);
            Assert.That(model!.Title, Is.EqualTo("Admin Edit Test"));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNullForMissingBuild()
        {
            var model = await service.GetForEditAsync(
                -1,
                AuthorId,
                false);

            Assert.That(model, Is.Null);
        }

        [Test]
        public async Task ChampionExistsAsync_ReturnsTrueForExistingChampion()
        {
            var result = await service.ChampionExistsAsync(championId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ChampionExistsAsync_ReturnsFalseForMissingChampion()
        {
            var result = await service.ChampionExistsAsync(-1);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ItemsExistAsync_ReturnsTrueWhenAllItemsExist()
        {
            var result = await service.ItemsExistAsync(new[] { itemId });

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ItemsExistAsync_ReturnsFalseWhenAnyItemIsMissing()
        {
            var result = await service.ItemsExistAsync(new[] { itemId, -1 });

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetFilteredAsync_FiltersByTitle()
        {
            var matchingBuild = await AddBuildAsync(
                AuthorId,
                "Unique Search Build");

            await AddBuildAsync(AuthorId, "Another Build");

            var model = new BuildQueryViewModel
            {
                SearchTerm = "  Unique Search  "
            };

            await service.GetFilteredAsync(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.SearchTerm, Is.EqualTo("Unique Search"));
                Assert.That(model.TotalBuilds, Is.EqualTo(1));

                Assert.That(
                    model.Builds.Select(build => build.Id),
                    Is.EqualTo(new[] { matchingBuild.Id }));
            });
        }

        [Test]
        public async Task GetFilteredAsync_CombinesAllFilters()
        {
            var otherChampion = new Champion
            {
                Name = "Another Filter Champion",
                Description = "Champion for filtering tests.",
                ImageUrl = "/images/champions/another.jpg"
            };

            database.Context.Champions.Add(otherChampion);
            await database.Context.SaveChangesAsync();

            var matchingBuild = await AddBuildAsync(
                AuthorId,
                "Combined Filter Match");

            var wrongPosition = await AddBuildAsync(
                AuthorId,
                "Combined Filter Wrong Position");

            wrongPosition.Position = Position.Mid;

            var wrongChampion = await AddBuildAsync(
                AuthorId,
                "Combined Filter Wrong Champion");

            wrongChampion.ChampionId = otherChampion.Id;

            await AddBuildAsync(AuthorId, "Unrelated Title");
            await database.Context.SaveChangesAsync();

            var model = new BuildQueryViewModel
            {
                SearchTerm = "Combined Filter",
                ChampionId = championId,
                Position = Position.Top
            };

            await service.GetFilteredAsync(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.TotalBuilds, Is.EqualTo(1));

                Assert.That(
                    model.Builds.Select(build => build.Id),
                    Is.EqualTo(new[] { matchingBuild.Id }));
            });
        }

        [Test]
        public async Task GetFilteredAsync_ReturnsEmptyResultsWhenNothingMatches()
        {
            await AddBuildAsync(AuthorId, "Existing Build");

            var model = new BuildQueryViewModel
            {
                SearchTerm = "No Such Build 987654",
                CurrentPage = 10
            };

            await service.GetFilteredAsync(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.Builds, Is.Empty);
                Assert.That(model.TotalBuilds, Is.Zero);
                Assert.That(model.TotalPages, Is.Zero);
                Assert.That(model.CurrentPage, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task GetFilteredAsync_ReturnsPagesInNewestFirstOrder()
        {
            var createdBuilds = new List<Build>();
            var startDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            for (int i = 1; i <= 8; i++)
            {
                var build = await AddBuildAsync(
                    AuthorId,
                    $"Pagination Build {i}");

                build.CreatedOn = startDate.AddDays(i);
                createdBuilds.Add(build);
            }

            await database.Context.SaveChangesAsync();

            var firstPage = new BuildQueryViewModel
            {
                SearchTerm = "Pagination Build",
                CurrentPage = 1
            };

            var secondPage = new BuildQueryViewModel
            {
                SearchTerm = "Pagination Build",
                CurrentPage = 2
            };

            await service.GetFilteredAsync(firstPage);
            await service.GetFilteredAsync(secondPage);

            var expectedIds = createdBuilds
                .OrderByDescending(build => build.CreatedOn)
                .Select(build => build.Id)
                .ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(firstPage.TotalBuilds, Is.EqualTo(8));
                Assert.That(firstPage.TotalPages, Is.EqualTo(2));

                Assert.That(
                    firstPage.Builds.Select(build => build.Id),
                    Is.EqualTo(expectedIds.Take(6)));

                Assert.That(secondPage.CurrentPage, Is.EqualTo(2));

                Assert.That(
                    secondPage.Builds.Select(build => build.Id),
                    Is.EqualTo(expectedIds.Skip(6)));
            });
        }

        [Test]
        public async Task GetFilteredAsync_OrdersEqualDatesByDescendingId()
        {
            var firstBuild = await AddBuildAsync(AuthorId, "Same Date First");
            var secondBuild = await AddBuildAsync(AuthorId, "Same Date Second");

            var sameDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            firstBuild.CreatedOn = sameDate;
            secondBuild.CreatedOn = sameDate;

            await database.Context.SaveChangesAsync();

            var model = new BuildQueryViewModel
            {
                SearchTerm = "Same Date"
            };

            await service.GetFilteredAsync(model);

            Assert.That(
                model.Builds.Select(build => build.Id),
                Is.EqualTo(new[] { secondBuild.Id, firstBuild.Id }));
        }

        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(100)]
        public async Task GetFilteredAsync_AdjustsPageOutsideAvailableRange(
            int requestedPage)
        {
            var build = await AddBuildAsync(AuthorId, "Page Boundary Build");

            var model = new BuildQueryViewModel
            {
                SearchTerm = "Page Boundary",
                CurrentPage = requestedPage
            };

            await service.GetFilteredAsync(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.CurrentPage, Is.EqualTo(1));
                Assert.That(model.TotalBuilds, Is.EqualTo(1));

                Assert.That(
                    model.Builds.Select(b => b.Id),
                    Is.EqualTo(new[] { build.Id }));
            });
        }

        private async Task<Build> AddBuildAsync(
            string authorId,
            string title)
        {
            var build = new Build
            {
                Title = title,
                Description = "Original description.",
                AuthorId = authorId,
                ChampionId = championId,
                Position = Position.Top
            };

            build.BuildItems.Add(new BuildItem
            {
                ItemId = itemId,
                Slot = 1
            });

            database.Context.Builds.Add(build);
            await database.Context.SaveChangesAsync();

            return build;
        }
    }
}