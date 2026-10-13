using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RiftBuilds.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 13, "An offensive option for builds focused on ability power.", "/images/items/rabadons-deathcap.png", "Rabadon's Deathcap", 3600 },
                    { 14, "A defensive option for mages who need protection during dangerous fights.", "/images/items/zhonyas-hourglass.png", "Zhonya's Hourglass", 3250 },
                    { 15, "A magic penetration option for fighting enemies with magic resistance.", "/images/items/void-staff.png", "Void Staff", 3000 },
                    { 16, "A classic mage item for builds focused on burst magic damage.", "/images/items/ludens-echo.png", "Luden's Echo", 3200 },
                    { 17, "A versatile offensive option for combining abilities with basic attacks.", "/images/items/trinity-force.png", "Trinity Force", 3333 },
                    { 18, "An attack-focused option for sustained damage and recovering health.", "/images/items/blade-of-the-ruined-king.png", "Blade of the Ruined King", 3200 },
                    { 19, "An offensive option for physical damage builds that benefit from mobility.", "/images/items/youmuus-ghostblade.png", "Youmuu's Ghostblade", 2800 },
                    { 20, "A fighter item that combines physical offense with defensive utility.", "/images/items/deaths-dance.png", "Death's Dance", 3300 },
                    { 21, "A defensive option for magic resistance and stronger healing effects.", "/images/items/spirit-visage.png", "Spirit Visage", 2700 },
                    { 22, "A durable frontline option for fighting enemies at close range.", "/images/items/sunfire-aegis.png", "Sunfire Aegis", 2700 },
                    { 23, "A defensive option for facing enemies who rely on critical strikes.", "/images/items/randuins-omen.png", "Randuin's Omen", 2700 },
                    { 24, "A support option for protecting nearby allies during team fights.", "/images/items/locket-of-the-iron-solari.png", "Locket of the Iron Solari", 2200 },
                    { 25, "A protective support option for helping a chosen ally survive.", "/images/items/knights-vow.png", "Knight's Vow", 2300 },
                    { 26, "A support option for helping an ally recover from disabling effects.", "/images/items/mikaels-blessing.png", "Mikael's Blessing", 2300 },
                    { 27, "Defensive boots for magic resistance and resistance to crowd control.", "/images/items/mercurys-treads.png", "Mercury's Treads", 1300 },
                    { 28, "Offensive boots for builds focused on magic damage.", "/images/items/sorcerers-shoes.png", "Sorcerer's Shoes", 1100 },
                    { 29, "An offensive and defensive option that offers a second chance in combat.", "/images/items/guardian-angel.png", "Guardian Angel", 3200 },
                    { 30, "An offensive option for combining ability power with frequent basic attacks.", "/images/items/nashors-tooth.png", "Nashor's Tooth", 3000 },
                    { 31, "An offensive option for rapid attacks and builds focused on on-hit effects.", "/images/items/guinsoos-rageblade.png", "Guinsoo's Rageblade", 3000 },
                    { 32, "An armor penetration option for physical damage and critical strike builds.", "/images/items/lord-dominiks-regards.png", "Lord Dominik's Regards", 3100 },
                    { 33, "An attack-speed option for setting up attacks from a safer distance.", "/images/items/rapid-firecannon.png", "Rapid Firecannon", 2650 },
                    { 34, "A magic damage option for wearing down enemies during extended fights.", "/images/items/liandrys-torment.png", "Liandry's Torment", 3000 },
                    { 35, "A defensive option for armor, mana, and facing fast-attacking enemies.", "/images/items/frozen-heart.png", "Frozen Heart", 2500 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 35);
        }
    }
}
