using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kütüphane_Yonetim_Sistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddTCNumberToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TCNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TCNumber",
                table: "Users");
        }
    }
}
