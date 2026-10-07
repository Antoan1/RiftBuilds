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

            modelBuilder.Entity<Item>().HasData(
    new Item
    {
        Id = 1,
        Name = "Black Cleaver",
        Description = "Physical offense with extra durability.",
        Price = 3000,
        ImageUrl = "/images/items/black-cleaver.png"
    },
    new Item
    {
        Id = 2,
        Name = "Sterak's Gage",
        Description = "Extra protection during dangerous fights.",
        Price = 3200,
        ImageUrl = "/images/items/steraks-gage.png"
    },
    new Item
    {
        Id = 3,
        Name = "Infinity Edge",
        Description = "An offensive option for critical strike builds.",
        Price = 3600,
        ImageUrl = "/images/items/infinity-edge.png"
    },
    new Item
    {
        Id = 4,
        Name = "Phantom Dancer",
        Description = "Faster attacks and improved mobility.",
        Price = 2650,
        ImageUrl = "/images/items/phantom-dancer.png"
    },
    new Item
    {
        Id = 5,
        Name = "Bloodthirster",
        Description = "An offensive option for recovering health through attacks.",
        Price = 3400,
        ImageUrl = "/images/items/bloodthirster.png"
    },
    new Item
    {
        Id = 6,
        Name = "Runaan's Hurricane",
        Description = "A ranged combat option for attacking multiple targets.",
        Price = 2650,
        ImageUrl = "/images/items/runaans-hurricane.png"
    },
    new Item
    {
        Id = 7,
        Name = "Thornmail",
        Description = "A defensive option against enemy attackers.",
        Price = 2450,
        ImageUrl = "/images/items/thornmail.png"
    },
    new Item
    {
        Id = 8,
        Name = "Abyssal Mask",
        Description = "A defensive option for fighting nearby magic threats.",
        Price = 2650,
        ImageUrl = "/images/items/abyssal-mask.png"
    },
    new Item
    {
        Id = 9,
        Name = "Ardent Censer",
        Description = "A support option for empowering allied attackers.",
        Price = 2300,
        ImageUrl = "/images/items/ardent-censer.png"
    },
    new Item
    {
        Id = 10,
        Name = "Redemption",
        Description = "A support option for helping the team recover health.",
        Price = 2300,
        ImageUrl = "/images/items/redemption.png"
    },
    new Item
    {
        Id = 11,
        Name = "Plated Steelcaps",
        Description = "Defensive boots for facing physical attackers.",
        Price = 1200,
        ImageUrl = "/images/items/plated-steelcaps.png"
    },
    new Item
    {
        Id = 12,
        Name = "Berserker's Greaves",
        Description = "Offensive boots for faster basic attacks.",
        Price = 1100,
        ImageUrl = "/images/items/berserkers-greaves.png"
    }
);
        }

    }
}
