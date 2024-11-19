using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addToeicAttemped4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToeicPracticeRecords_Attempt",
                table: "Toeic_Practice_Records");

            migrationBuilder.DropTable(
                name: "Toeic_ToeicAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Toeic_Practice_Records_AttemptId",
                table: "Toeic_Practice_Records");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Toeic_Attempts",
                columns: table => new
                {
                    AttemptId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToeicId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ListeningScore = table.Column<int>(type: "int", nullable: true),
                    ReadingScore = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_Attempts", x => x.AttemptId);
                    table.ForeignKey(
                        name: "FK_ToeicAttempted_ToeicExams",
                        column: x => x.ToeicId,
                        principalTable: "Toeic_Exams",
                        principalColumn: "ToeicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToeicAttempted_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Practice_Records_AttemptId",
                table: "Toeic_Practice_Records",
                column: "AttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Attempts_ToeicId",
                table: "Toeic_Attempts",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Attempts_UserId",
                table: "Toeic_Attempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToeicPracticeRecords_Attempt",
                table: "Toeic_Practice_Records",
                column: "AttemptId",
                principalTable: "Toeic_Attempts",
                principalColumn: "AttemptId");
        }
    }
}
