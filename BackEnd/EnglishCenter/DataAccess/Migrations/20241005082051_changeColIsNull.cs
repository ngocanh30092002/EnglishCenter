using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class changeColIsNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ques_LC_Answer_Image",
                table: "Ques_LC_Image");

            migrationBuilder.DropIndex(
                name: "IX_Ques_LC_Image_AnswerId",
                table: "Ques_LC_Image");

            migrationBuilder.AlterColumn<long>(
                name: "AnswerId",
                table: "Ques_LC_Image",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Ques_LC_Image_AnswerId",
                table: "Ques_LC_Image",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Ques_LC_Answer_Image",
                table: "Ques_LC_Image",
                column: "AnswerId",
                principalTable: "Answer_LC_Image",
                principalColumn: "AnswerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ques_LC_Answer_Image",
                table: "Ques_LC_Image");

            migrationBuilder.DropIndex(
                name: "IX_Ques_LC_Image_AnswerId",
                table: "Ques_LC_Image");

            migrationBuilder.AlterColumn<long>(
                name: "AnswerId",
                table: "Ques_LC_Image",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
    }
}
