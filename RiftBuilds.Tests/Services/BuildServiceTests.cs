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