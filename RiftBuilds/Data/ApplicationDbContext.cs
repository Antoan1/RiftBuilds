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
    },
    new Champion
    {
        Id = 6,
        Name = "Darius",
        Description = "An axe-wielding fighter who pressures nearby enemies and excels in extended battles.",
        ImageUrl = "/images/champions/darius.jpg"
    },
    new Champion
    {
        Id = 7,
        Name = "Garen",
        Description = "A resilient warrior who charges into combat with a greatsword and finishes weakened opponents.",
        ImageUrl = "/images/champions/garen.jpg"
    },
    new Champion
    {
        Id = 8,
        Name = "Fiora",
        Description = "A precise duelist who exploits weak points and counters incoming attacks.",
        ImageUrl = "/images/champions/fiora.jpg"
    },
    new Champion
    {
        Id = 9,
        Name = "Malphite",
        Description = "A durable frontline champion who disrupts enemies and initiates team fights.",
        ImageUrl = "/images/champions/malphite.jpg"
    },
    new Champion
    {
        Id = 10,
        Name = "Lee Sin",
        Description = "A mobile martial artist who combines quick movement with powerful strikes and kicks.",
        ImageUrl = "/images/champions/lee-sin.jpg"
    },
    new Champion
    {
        Id = 11,
        Name = "Vi",
        Description = "An aggressive fighter who uses powerful gauntlets to break through defenses and reach priority targets.",
        ImageUrl = "/images/champions/vi.jpg"
    },
    new Champion
    {
        Id = 12,
        Name = "Warwick",
        Description = "A relentless hunter who tracks wounded enemies and sustains himself through close combat.",
        ImageUrl = "/images/champions/warwick.jpg"
    },
    new Champion
    {
        Id = 13,
        Name = "Sejuani",
        Description = "A mounted frontline champion who uses frost and crowd control to set up fights for her team.",
        ImageUrl = "/images/champions/sejuani.jpg"
    },
    new Champion
    {
        Id = 14,
        Name = "Ahri",
        Description = "A mobile mage who uses charm and magical attacks to catch enemies and reposition during fights.",
        ImageUrl = "/images/champions/ahri.jpg"
    },
    new Champion
    {
        Id = 15,
        Name = "Zed",
        Description = "An assassin who uses shadows to outmaneuver opponents and threaten vulnerable targets.",
        ImageUrl = "/images/champions/zed.jpg"
    },
    new Champion
    {
        Id = 16,
        Name = "Lux",
        Description = "A long-range mage who combines light-based attacks, protective shields, and crowd control.",
        ImageUrl = "/images/champions/lux.jpg"
    },
    new Champion
    {
        Id = 17,
        Name = "Syndra",
        Description = "A mage who controls dark spheres to pressure enemies and deliver concentrated magical damage.",
        ImageUrl = "/images/champions/syndra.jpg"
    },
    new Champion
    {
        Id = 18,
        Name = "Caitlyn",
        Description = "A long-range marksman who uses careful positioning and traps to control the battlefield.",
        ImageUrl = "/images/champions/caitlyn.jpg"
    },
    new Champion
    {
        Id = 19,
        Name = "Ashe",
        Description = "A marksman who slows enemies with frost arrows and helps her team with vision and initiation.",
        ImageUrl = "/images/champions/ashe.jpg"
    },
    new Champion
    {
        Id = 20,
        Name = "Ezreal",
        Description = "A mobile marksman who relies on aimed abilities and repositioning to deal damage from a distance.",
        ImageUrl = "/images/champions/ezreal.jpg"
    },
    new Champion
    {
        Id = 21,
        Name = "Kai'Sa",
        Description = "An adaptable marksman who combines weapon attacks and Void abilities to pursue exposed targets.",
        ImageUrl = "/images/champions/kaisa.jpg"
    },
    new Champion
    {
        Id = 22,
        Name = "Leona",
        Description = "An armored support who initiates fights and holds enemies in place with crowd control.",
        ImageUrl = "/images/champions/leona.jpg"
    },
    new Champion
    {
        Id = 23,
        Name = "Thresh",
        Description = "A support who catches enemies with his hook and helps allies escape using his lantern.",
        ImageUrl = "/images/champions/thresh.jpg"
    },
    new Champion
    {
        Id = 24,
        Name = "Nami",
        Description = "A support mage who uses water magic to heal allies, enhance attacks, and disrupt enemies.",
        ImageUrl = "/images/champions/nami.jpg"
    },
    new Champion
    {
        Id = 25,
        Name = "Nautilus",
        Description = "A heavily armored support who uses his anchor and crowd control to engage and protect allies.",
        ImageUrl = "/images/champions/nautilus.jpg"
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
