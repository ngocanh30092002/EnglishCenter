using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addColInCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageThumbnail",
                table: "Courses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageThumbnail",
                table: "Courses");
        }
    }
}
