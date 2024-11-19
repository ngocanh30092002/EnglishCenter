using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addColInDirection1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introduce",
                table: "Toeic_Direction",
                newName: "Introduce_Reading");

            migrationBuilder.AddColumn<string>(
                name: "Introduce_Listening",
                table: "Toeic_Direction",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduce_Listening",
                table: "Toeic_Direction");

            migrationBuilder.RenameColumn(
                name: "Introduce_Reading",
                table: "Toeic_Direction",
                newName: "Introduce");
        }
    }
}
