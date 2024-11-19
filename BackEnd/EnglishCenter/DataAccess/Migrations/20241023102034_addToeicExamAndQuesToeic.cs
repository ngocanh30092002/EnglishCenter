using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addToeicExamAndQuesToeic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Toeic_Exams",
                columns: table => new
                {
                    ToeicId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Exams", x => x.ToeicId);
                });

            migrationBuilder.CreateTable(
                name: "Ques_Toeic",
                columns: table => new
                {
                    QuesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<long>(type: "bigint", nullable: false),
                    NoNum = table.Column<int>(type: "int", nullable: false),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGroup = table.Column<bool>(type: "bit", nullable: false),
                    Part = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ques_Toeic", x => x.QuesId);
                    table.ForeignKey(
                        name: "FK_Ques_Toeic_Toeic_Exam",
                        column: x => x.ToeicId,
                        principalTable: "Toeic_Exams",
                        principalColumn: "ToeicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ques_Toeic_ToeicId",
                table: "Ques_Toeic",
                column: "ToeicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ques_Toeic");

            migrationBuilder.DropTable(
                name: "Toeic_Exams");
        }
    }
}
