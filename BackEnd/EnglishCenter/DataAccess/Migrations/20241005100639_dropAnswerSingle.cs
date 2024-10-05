using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class dropAnswerSingle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sub_Ques_RC_Answer_Single",
                table: "Sub_RC_Single");

            migrationBuilder.DropTable(
                name: "AnswerRcSingle");

            migrationBuilder.DropIndex(
                name: "IX_Sub_RC_Single_AnswerId",
                table: "Sub_RC_Single");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Sub_RC_Single");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AnswerId",
                table: "Sub_RC_Single",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer_RC_Single",
                columns: table => new
                {
                    AnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer_RC_Single", x => x.AnswerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sub_RC_Single_AnswerId",
                table: "Sub_RC_Single",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Sub_Ques_RC_Answer_Single",
                table: "Sub_RC_Single",
                column: "AnswerId",
                principalTable: "Answer_RC_Single",
                principalColumn: "AnswerId");
        }
    }
}
