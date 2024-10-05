using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class answerForConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Sub_LC_Conversation");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Ques_LC_Audio");

            migrationBuilder.AddColumn<long>(
                name: "AnswerId",
                table: "Sub_LC_Conversation",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer_LC_Conversation",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer_LC_Conversation", x => x.AnswerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sub_LC_Conversation_AnswerId",
                table: "Sub_LC_Conversation",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Sub_Ques_LC_Answer_Conversation",
                table: "Sub_LC_Conversation",
                column: "AnswerId",
                principalTable: "Answer_LC_Conversation",
                principalColumn: "AnswerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sub_Ques_LC_Answer_Conversation",
                table: "Sub_LC_Conversation");

            migrationBuilder.DropTable(
                name: "Answer_LC_Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Sub_LC_Conversation_AnswerId",
                table: "Sub_LC_Conversation");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Sub_LC_Conversation");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Sub_LC_Conversation",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Ques_LC_Audio",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }
    }
}
