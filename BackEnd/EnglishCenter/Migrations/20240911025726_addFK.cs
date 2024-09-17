using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assign_Ques_Assignment",
                table: "Assign_Ques");

            migrationBuilder.AddColumn<long>(
                name: "AssignmentId",
                table: "Assign_Ques",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_AssignmentId",
                table: "Assign_Ques",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assign_Ques_Assignment",
                table: "Assign_Ques",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assign_Ques_Ques_LC_Image",
                table: "Assign_Ques",
                column: "Ques_Id",
                principalTable: "Ques_LC_Image",
                principalColumn: "QuesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assign_Ques_Assignment",
                table: "Assign_Ques");

            migrationBuilder.DropForeignKey(
                name: "FK_Assign_Ques_Ques_LC_Image",
                table: "Assign_Ques");

            migrationBuilder.DropIndex(
                name: "IX_Assign_Ques_AssignmentId",
                table: "Assign_Ques");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Assign_Ques");

            migrationBuilder.AddForeignKey(
                name: "FK_Assign_Ques_Assignment",
                table: "Assign_Ques",
                column: "Ques_Id",
                principalTable: "Ques_LC_Image",
                principalColumn: "QuesId");
        }
    }
}
