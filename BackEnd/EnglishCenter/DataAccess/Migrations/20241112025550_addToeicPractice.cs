using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addToeicPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Toeic_Practice_Records",
                columns: table => new
                {
                    RecordId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SubQueId = table.Column<long>(type: "bigint", nullable: false),
                    SelectedAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    ToeicId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Practice_Records", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_ToeicPracticeRecord_SubToeic",
                        column: x => x.SubQueId,
                        principalTable: "Sub_Toeic",
                        principalColumn: "SubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToeicPracticeRecord_ToeicExam",
                        column: x => x.ToeicId,
                        principalTable: "Toeic_Exams",
                        principalColumn: "ToeicId");
                    table.ForeignKey(
                        name: "FK_ToeicPracticeRecord_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Practice_Records_SubQueId",
                table: "Toeic_Practice_Records",
                column: "SubQueId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Practice_Records_ToeicId",
                table: "Toeic_Practice_Records",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Practice_Records_UserId",
                table: "Toeic_Practice_Records",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Toeic_Practice_Records");
        }
    }
}
