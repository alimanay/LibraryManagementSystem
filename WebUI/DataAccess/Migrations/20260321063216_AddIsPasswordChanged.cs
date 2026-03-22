using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPasswordChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {     
               migrationBuilder.AddColumn<bool>(
               name: "IsPasswordChanged",
               table: "Users",
               type: "bit",
               nullable: false,
               defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         migrationBuilder.DropColumn(
        name: "IsPasswordChanged",
        table: "Users");
        }
    }
}
