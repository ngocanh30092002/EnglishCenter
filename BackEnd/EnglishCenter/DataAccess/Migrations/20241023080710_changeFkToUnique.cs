using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class changeFkToUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Examinations_ContentId",
                table: "Examinations");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_ContentId",
                table: "Examinations",
                column: "ContentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Examinations_ContentId",
                table: "Examinations");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_ContentId",
                table: "Examinations",
                column: "ContentId");
        }
    }
}
