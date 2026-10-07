using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RiftBuilds.Migrations
{
    /// <inheritdoc />
    public partial class SeedChampions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Champions",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "A versatile champion who switches between ranged combat and a powerful melee transformation.", "/images/champions/gnar.jpg", "Gnar" },
                    { 2, "A mobile swordsman who uses wind techniques and precise attacks to defeat his enemies.", "/images/champions/yasuo.jpg", "Yasuo" },
                    { 3, "A durable champion who locks down enemies and helps his team with powerful crowd control.", "/images/champions/amumu.jpg", "Amumu" },
                    { 4, "A ranged damage dealer who switches weapons and becomes increasingly dangerous during team fights.", "/images/champions/jinx.jpg", "Jinx" },
                    { 5, "A support champion who protects allies with shields and empowers them with magical abilities.", "/images/champions/lulu.jpg", "Lulu" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Champions",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
