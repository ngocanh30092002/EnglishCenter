using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addTblRcSentence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SentenceQuesId",
                table: "Assign_Ques",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnswerRcSentence",
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
                    table.PrimaryKey("PK_AnswerRcSentence", x => x.AnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Ques_RC_Sentence",
                columns: table => new
                {
                    QuesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerA = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnswerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ques_RC_Sentence", x => x.QuesId);
                    table.ForeignKey(
                        name: "FK_Ques_RC_Answer_Sentence",
                        column: x => x.AnswerId,
                        principalTable: "AnswerRcSentence",
                        principalColumn: "AnswerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_SentenceQuesId",
                table: "Assign_Ques",
                column: "SentenceQuesId");

            migrationBuilder.CreateIndex(
                name: "IX_Ques_RC_Sentence_AnswerId",
                table: "Ques_RC_Sentence",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Assign_Ques_Ques_RC_Sentence",
                table: "Assign_Ques",
                column: "SentenceQuesId",
                principalTable: "Ques_RC_Sentence",
                principalColumn: "QuesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assign_Ques_Ques_RC_Sentence",
                table: "Assign_Ques");

            migrationBuilder.DropTable(
                name: "Ques_RC_Sentence");

            migrationBuilder.DropTable(
                name: "AnswerRcSentence");

            migrationBuilder.DropIndex(
                name: "IX_Assign_Ques_SentenceQuesId",
                table: "Assign_Ques");

            migrationBuilder.DropColumn(
                name: "SentenceQuesId",
                table: "Assign_Ques");
        }
    }
}
