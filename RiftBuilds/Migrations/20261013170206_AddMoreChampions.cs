using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RiftBuilds.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreChampions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.InsertData(
                table: "Champions",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 6, "An axe-wielding fighter who pressures nearby enemies and excels in extended battles.", "/images/champions/darius.jpg", "Darius" },
                    { 7, "A resilient warrior who charges into combat with a greatsword and finishes weakened opponents.", "/images/champions/garen.jpg", "Garen" },
                    { 8, "A precise duelist who exploits weak points and counters incoming attacks.", "/images/champions/fiora.jpg", "Fiora" },
                    { 9, "A durable frontline champion who disrupts enemies and initiates team fights.", "/images/champions/malphite.jpg", "Malphite" },
                    { 10, "A mobile martial artist who combines quick movement with powerful strikes and kicks.", "/images/champions/lee-sin.jpg", "Lee Sin" },
                    { 11, "An aggressive fighter who uses powerful gauntlets to break through defenses and reach priority targets.", "/images/champions/vi.jpg", "Vi" },
                    { 12, "A relentless hunter who tracks wounded enemies and sustains himself through close combat.", "/images/champions/warwick.jpg", "Warwick" },
                    { 13, "A mounted frontline champion who uses frost and crowd control to set up fights for her team.", "/images/champions/sejuani.jpg", "Sejuani" },
                    { 14, "A mobile mage who uses charm and magical attacks to catch enemies and reposition during fights.", "/images/champions/ahri.jpg", "Ahri" },
                    { 15, "An assassin who uses shadows to outmaneuver opponents and threaten vulnerable targets.", "/images/champions/zed.jpg", "Zed" },
                    { 16, "A long-range mage who combines light-based attacks, protective shields, and crowd control.", "/images/champions/lux.jpg", "Lux" },
                    { 17, "A mage who controls dark spheres to pressure enemies and deliver concentrated magical damage.", "/images/champions/syndra.jpg", "Syndra" },
                    { 18, "A long-range marksman who uses careful positioning and traps to control the battlefield.", "/images/champions/caitlyn.jpg", "Caitlyn" },
                    { 19, "A marksman who slows enemies with frost arrows and helps her team with vision and initiation.", "/images/champions/ashe.jpg", "Ashe" },
                    { 20, "A mobile marksman who relies on aimed abilities and repositioning to deal damage from a distance.", "/images/champions/ezreal.jpg", "Ezreal" },
                    { 21, "An adaptable marksman who combines weapon attacks and Void abilities to pursue exposed targets.", "/images/champions/kaisa.jpg", "Kai'Sa" },
                    { 22, "An armored support who initiates fights and holds enemies in place with crowd control.", "/images/champions/leona.jpg", "Leona" },
                    { 23, "A support who catches enemies with his hook and helps allies escape using his lantern.", "/images/champions/thresh.jpg", "Thresh" },
                    { 24, "A support mage who uses water magic to heal allies, enhance attacks, and disrupt enemies.", "/images/champions/nami.jpg", "Nami" },
                    { 25, "A heavily armored support who uses his anchor and crowd control to engage and protect allies.", "/images/champions/nautilus.jpg", "Nautilus" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
