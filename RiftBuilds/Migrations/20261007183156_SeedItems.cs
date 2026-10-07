using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RiftBuilds.Migrations
{
    /// <inheritdoc />
    public partial class SeedItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Physical offense with extra durability.", "/images/items/black-cleaver.png", "Black Cleaver", 3000 },
                    { 2, "Extra protection during dangerous fights.", "/images/items/steraks-gage.png", "Sterak's Gage", 3200 },
                    { 3, "An offensive option for critical strike builds.", "/images/items/infinity-edge.png", "Infinity Edge", 3600 },
                    { 4, "Faster attacks and improved mobility.", "/images/items/phantom-dancer.png", "Phantom Dancer", 2650 },
                    { 5, "An offensive option for recovering health through attacks.", "/images/items/bloodthirster.png", "Bloodthirster", 3400 },
                    { 6, "A ranged combat option for attacking multiple targets.", "/images/items/runaans-hurricane.png", "Runaan's Hurricane", 2650 },
                    { 7, "A defensive option against enemy attackers.", "/images/items/thornmail.png", "Thornmail", 2450 },
                    { 8, "A defensive option for fighting nearby magic threats.", "/images/items/abyssal-mask.png", "Abyssal Mask", 2650 },
                    { 9, "A support option for empowering allied attackers.", "/images/items/ardent-censer.png", "Ardent Censer", 2300 },
                    { 10, "A support option for helping the team recover health.", "/images/items/redemption.png", "Redemption", 2300 },
                    { 11, "Defensive boots for facing physical attackers.", "/images/items/plated-steelcaps.png", "Plated Steelcaps", 1200 },
                    { 12, "Offensive boots for faster basic attacks.", "/images/items/berserkers-greaves.png", "Berserker's Greaves", 1100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
