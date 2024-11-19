using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class ToeicDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DirectionId",
                table: "Toeic_Exams",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Toeic_Direction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Introduce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Part1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audio1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Part2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audio2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Part3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audio3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Part4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audio4 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Direction", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Exams_DirectionId",
                table: "Toeic_Exams",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Toeic_Exam_Toeic_Directions",
                table: "Toeic_Exams",
                column: "DirectionId",
                principalTable: "Toeic_Direction",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Toeic_Exam_Toeic_Directions",
                table: "Toeic_Exams");

            migrationBuilder.DropTable(
                name: "Toeic_Direction");

            migrationBuilder.DropIndex(
                name: "IX_Toeic_Exams_DirectionId",
                table: "Toeic_Exams");

            migrationBuilder.DropColumn(
                name: "DirectionId",
                table: "Toeic_Exams");
        }
    }
}
