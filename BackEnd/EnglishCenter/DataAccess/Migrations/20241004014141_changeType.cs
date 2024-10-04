using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class changeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuesTypeId",
                table: "Assign_Ques",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Assign_Ques",
                newName: "QuesTypeId");
        }
    }
}
