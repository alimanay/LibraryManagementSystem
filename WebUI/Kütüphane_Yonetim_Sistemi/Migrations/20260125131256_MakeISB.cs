using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kütüphane_Yonetim_Sistemi.Migrations
{
    /// <inheritdoc />
    public partial class MakeISB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableCopies",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TotalCopies",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableCopies",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalCopies",
                table: "Books",
                type: "int",
                nullable: true);
        }
    }
}
