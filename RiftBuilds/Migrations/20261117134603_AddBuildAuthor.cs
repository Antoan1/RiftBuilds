using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiftBuilds.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Builds",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Builds_AuthorId",
                table: "Builds",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Builds_AspNetUsers_AuthorId",
                table: "Builds",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Builds_AspNetUsers_AuthorId",
                table: "Builds");

            migrationBuilder.DropIndex(
                name: "IX_Builds_AuthorId",
                table: "Builds");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Builds");
        }
    }
}
