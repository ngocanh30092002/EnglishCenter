using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addToeicAttemped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToeicPracticeRecord_ToeicExam",
                table: "Toeic_Practice_Records");

            migrationBuilder.DropForeignKey(
                name: "FK_ToeicPracticeRecord_User",
                table: "Toeic_Practice_Records");

            migrationBuilder.DropIndex(
                name: "IX_Toeic_Practice_Records_ToeicId",
                table: "Toeic_Practice_Records");

            migrationBuilder.DropIndex(
                name: "IX_Toeic_Practice_Records_UserId",
                table: "Toeic_Practice_Records");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Toeic_Practice_Records");

            migrationBuilder.AddColumn<long>(
                name: "AttemptId",
                table: "Toeic_Practice_Records",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Toeic_ToeicAttempts",
                columns: table => new
                {
                    AttemptId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToeicId = table.Column<long>(type: "bigint", nullable: false),
                    ListeningScore = table.Column<int>(type: "int", nullable: true),
                    ReadingScore = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toeic_ToeicAttempts", x => x.AttemptId);
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
                name: "IX_Toeic_ToeicAttempts_ToeicId",
                table: "Toeic_ToeicAttempts",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_ToeicAttempts_UserId",
                table: "Toeic_ToeicAttempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToeicPracticeRecords_Attempt",
                table: "Toeic_Practice_Records",
                column: "AttemptId",
                principalTable: "Toeic_ToeicAttempts",
                principalColumn: "AttemptId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToeicPracticeRecords_Attempt",
                table: "Toeic_Practice_Records");

            migrationBuilder.DropTable(
                name: "Toeic_ToeicAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Toeic_Practice_Records_AttemptId",
                table: "Toeic_Practice_Records");

            migrationBuilder.DropColumn(
                name: "AttemptId",
                table: "Toeic_Practice_Records");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Toeic_Practice_Records",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Practice_Records_ToeicId",
                table: "Toeic_Practice_Records",
                column: "ToeicId");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Practice_Records_UserId",
                table: "Toeic_Practice_Records",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToeicPracticeRecord_ToeicExam",
                table: "Toeic_Practice_Records",
                column: "ToeicId",
                principalTable: "Toeic_Exams",
                principalColumn: "ToeicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToeicPracticeRecord_User",
                table: "Toeic_Practice_Records",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
