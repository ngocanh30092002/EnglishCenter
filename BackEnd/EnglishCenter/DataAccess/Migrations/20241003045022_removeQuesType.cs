using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class removeQuesType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assign_Ques_Question_Type",
                table: "Assign_Ques");

            migrationBuilder.DropTable(
                name: "Question_Type");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Question_Type",
                columns: table => new
                {
                    QuesTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question_Type", x => x.QuesTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_QuesTypeId",
                table: "Assign_Ques",
                column: "QuesTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assign_Ques_Question_Type",
                table: "Assign_Ques",
                column: "QuesTypeId",
                principalTable: "Question_Type",
                principalColumn: "QuesTypeId");
        }
    }
}
