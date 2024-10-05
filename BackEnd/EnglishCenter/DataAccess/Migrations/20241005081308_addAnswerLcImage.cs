using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addAnswerLcImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AnswerId",
                table: "Ques_LC_Image",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Answer_LC_Image",
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
                    table.PrimaryKey("PK_Answer_LC_Image", x => x.AnswerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ques_LC_Image_AnswerId",
                table: "Ques_LC_Image",
                column: "AnswerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ques_LC_Answer_Image",
                table: "Ques_LC_Image",
                column: "AnswerId",
                principalTable: "Answer_LC_Image",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ques_LC_Answer_Image",
                table: "Ques_LC_Image");

            migrationBuilder.DropTable(
                name: "Answer_LC_Image");

            migrationBuilder.DropIndex(
                name: "IX_Ques_LC_Image_AnswerId",
                table: "Ques_LC_Image");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Ques_LC_Image");
        }
    }
}
