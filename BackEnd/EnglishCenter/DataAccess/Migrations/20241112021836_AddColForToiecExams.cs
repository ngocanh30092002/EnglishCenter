using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class AddColForToiecExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Completed_Num",
                table: "Toeic_Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "Toeic_Exams",
                type: "int",
                nullable: false,
                defaultValue: 990);

            migrationBuilder.AddColumn<int>(
                name: "TimeMinutes",
                table: "Toeic_Exams",
                type: "int",
                nullable: false,
                defaultValue: 120);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed_Num",
                table: "Toeic_Exams");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Toeic_Exams");

            migrationBuilder.DropColumn(
                name: "TimeMinutes",
                table: "Toeic_Exams");
        }
    }
}
