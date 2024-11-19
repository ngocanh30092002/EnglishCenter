using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addColInDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Part5",
                table: "Toeic_Direction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Part6",
                table: "Toeic_Direction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Part7",
                table: "Toeic_Direction",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Part5",
                table: "Toeic_Direction");

            migrationBuilder.DropColumn(
                name: "Part6",
                table: "Toeic_Direction");

            migrationBuilder.DropColumn(
                name: "Part7",
                table: "Toeic_Direction");
        }
    }
}
