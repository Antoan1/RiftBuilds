using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RiftBuilds.Data;

namespace RiftBuilds.Tests.Infrastructure
{
    public sealed class TestDatabase : IDisposable
    {
        private readonly SqliteConnection connection;

        public ApplicationDbContext Context { get; }

        public TestDatabase()
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            Context = new ApplicationDbContext(options);

            try
            {
                Context.Database.EnsureCreated();
            }
            catch
            {
                Context.Dispose();
                connection.Dispose();

                throw;
            }
        }

        public void Dispose()
        {
            Context.Dispose();
            connection.Dispose();
        }
    }
}