using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addUpdateCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Enrollment",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Enrollment");
        }
    }
}
