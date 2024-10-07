using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class dropAnswerLcAudio2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ques_LC_Answer_Audio",
                table: "Ques_LC_Audio");

            migrationBuilder.DropTable(
                name: "Answer_LC_Audio");

            migrationBuilder.DropIndex(
                name: "IX_Ques_LC_Audio_AnswerId",
                table: "Ques_LC_Audio");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Ques_LC_Audio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AnswerId",
                table: "Ques_LC_Audio",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer_LC_Audio",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer_LC_Audio", x => x.AnswerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ques_LC_Audio_AnswerId",
                table: "Ques_LC_Audio",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Ques_LC_Answer_Audio",
                table: "Ques_LC_Audio",
                column: "AnswerId",
                principalTable: "Answer_LC_Audio",
                principalColumn: "AnswerId");
        }
    }
}
