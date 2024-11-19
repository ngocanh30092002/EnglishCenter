using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class removeCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToeicQuesId",
                table: "Toeic_Records");

            migrationBuilder.CreateIndex(
                name: "IX_Toeic_Records_SubQueId",
                table: "Toeic_Records",
                column: "SubQueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToeicRecord_SubToeic",
                table: "Toeic_Records",
                column: "SubQueId",
                principalTable: "Sub_Toeic",
                principalColumn: "SubId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToeicRecord_SubToeic",
                table: "Toeic_Records");

            migrationBuilder.DropIndex(
                name: "IX_Toeic_Records_SubQueId",
                table: "Toeic_Records");

            migrationBuilder.AddColumn<long>(
                name: "ToeicQuesId",
                table: "Toeic_Records",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
