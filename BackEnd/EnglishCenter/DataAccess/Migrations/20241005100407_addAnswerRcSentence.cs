using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addAnswerRcSentence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AnswerId",
                table: "Ques_RC_Sentence",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer_RC_Sentence",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer_RC_Sentence", x => x.AnswerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ques_RC_Sentence_AnswerId",
                table: "Ques_RC_Sentence",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Ques_RC_Answer_Sentence",
                table: "Ques_RC_Sentence",
                column: "AnswerId",
                principalTable: "Answer_RC_Sentence",
                principalColumn: "AnswerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ques_RC_Answer_Sentence",
                table: "Ques_RC_Sentence");

            migrationBuilder.DropTable(
                name: "Answer_RC_Sentence");

            migrationBuilder.DropIndex(
                name: "IX_Ques_RC_Sentence_AnswerId",
                table: "Ques_RC_Sentence");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Ques_RC_Sentence");
        }
    }
}
