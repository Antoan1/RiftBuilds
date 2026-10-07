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

            modelBuilder.Entity<Champion>().HasData(
        new Champion
        {
            Id = 1,
            Name = "Gnar",
            Description = "A versatile champion who switches between ranged combat and a powerful melee transformation.",
            ImageUrl = "/images/champions/gnar.jpg"
        },
        new Champion
        {
            Id = 2,
            Name = "Yasuo",
            Description = "A mobile swordsman who uses wind techniques and precise attacks to defeat his enemies.",
            ImageUrl = "/images/champions/yasuo.jpg"
        },
        new Champion
        {
            Id = 3,
            Name = "Amumu",
            Description = "A durable champion who locks down enemies and helps his team with powerful crowd control.",
            ImageUrl = "/images/champions/amumu.jpg"
        },
        new Champion
        {
            Id = 4,
            Name = "Jinx",
            Description = "A ranged damage dealer who switches weapons and becomes increasingly dangerous during team fights.",
            ImageUrl = "/images/champions/jinx.jpg"
        },
        new Champion
        {
            Id = 5,
            Name = "Lulu",
            Description = "A support champion who protects allies with shields and empowers them with magical abilities.",
            ImageUrl = "/images/champions/lulu.jpg"
        }
    );
        }

    }
}
