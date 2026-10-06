using Microsoft.EntityFrameworkCore;
using RiftBuilds.Models;

namespace RiftBuilds.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Champion> Champions { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;

        public DbSet<Build> Builds { get; set; } = null!;

        public DbSet<BuildItem> BuildItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuildItem>()
                .HasKey(bi => new { bi.BuildId, bi.ItemId });
        }
    }
}