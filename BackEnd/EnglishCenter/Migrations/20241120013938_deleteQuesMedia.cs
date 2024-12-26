using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class deleteQuesMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assign_Ques_Ques_RC_Sentence_Media",
                table: "Assign_Ques");

            migrationBuilder.DropForeignKey(
                name: "FK_Home_Ques_Ques_RC_Sentence_Media",
                table: "Home_Ques");

            migrationBuilder.DropTable(
                name: "Ques_RC_Sentence_Media");

            migrationBuilder.DropTable(
                name: "Answer_RC_Sentence_Media");

            migrationBuilder.DropIndex(
                name: "IX_Home_Ques_SentenceMediaQuesId",
                table: "Home_Ques");

            migrationBuilder.DropIndex(
                name: "IX_Assign_Ques_SentenceMediaQuesId",
                table: "Assign_Ques");

            migrationBuilder.DropColumn(
                name: "SentenceMediaQuesId",
                table: "Home_Ques");

            migrationBuilder.DropColumn(
                name: "SentenceMediaQuesId",
                table: "Assign_Ques");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SentenceMediaQuesId",
                table: "Home_Ques",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SentenceMediaQuesId",
                table: "Assign_Ques",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer_RC_Sentence_Media",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer_RC_Sentence_Media", x => x.AnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Ques_RC_Sentence_Media",
                columns: table => new
                {
                    QuesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerId = table.Column<long>(type: "bigint", nullable: true),
                    AnswerA = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ques_RC_Sentence_Media", x => x.QuesId);
                    table.ForeignKey(
                        name: "FK_Ques_RC_Sentence_Media_Answer_RC_Sentence_Media_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer_RC_Sentence_Media",
                        principalColumn: "AnswerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_SentenceMediaQuesId",
                table: "Home_Ques",
                column: "SentenceMediaQuesId");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_SentenceMediaQuesId",
                table: "Assign_Ques",
                column: "SentenceMediaQuesId");

            migrationBuilder.CreateIndex(
                name: "IX_Ques_RC_Sentence_Media_AnswerId",
                table: "Ques_RC_Sentence_Media",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Assign_Ques_Ques_RC_Sentence_Media",
                table: "Assign_Ques",
                column: "SentenceMediaQuesId",
                principalTable: "Ques_RC_Sentence_Media",
                principalColumn: "QuesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Home_Ques_Ques_RC_Sentence_Media",
                table: "Home_Ques",
                column: "SentenceMediaQuesId",
                principalTable: "Ques_RC_Sentence_Media",
                principalColumn: "QuesId");
        }
    }
}
