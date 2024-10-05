using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addAnswerTriple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Sub_RC_Triple");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Sub_RC_Double");

            migrationBuilder.AddColumn<long>(
                name: "AnswerId",
                table: "Sub_RC_Triple",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer_RC_Triple",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer_RC_Triple", x => x.AnswerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sub_RC_Triple_AnswerId",
                table: "Sub_RC_Triple",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Sub_Ques_RC_Answer_Triple",
                table: "Sub_RC_Triple",
                column: "AnswerId",
                principalTable: "Answer_RC_Triple",
                principalColumn: "AnswerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sub_Ques_RC_Answer_Triple",
                table: "Sub_RC_Triple");

            migrationBuilder.DropTable(
                name: "Answer_RC_Triple");

            migrationBuilder.DropIndex(
                name: "IX_Sub_RC_Triple_AnswerId",
                table: "Sub_RC_Triple");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Sub_RC_Triple");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Sub_RC_Triple",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Sub_RC_Double",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }
    }
}
