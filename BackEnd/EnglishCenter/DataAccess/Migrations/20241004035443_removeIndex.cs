using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class removeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Assign_Ques",
                table: "Assign_Ques",
                newName: "IX_Assign_Ques_Ques_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Assign_Ques_Ques_Id",
                table: "Assign_Ques",
                newName: "IX_Assign_Ques");
        }
    }
}
