using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addColInSingle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoNum",
                table: "Sub_RC_Single",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "Ques_RC_Single",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoNum",
                table: "Sub_RC_Single");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Ques_RC_Single");
        }
    }
}
