using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class removeAssignQues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assign_Ques");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assign_Ques",
                columns: table => new
                {
                    AssignQuesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<long>(type: "bigint", nullable: true),
                    Ques_Id = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assign_Ques", x => x.AssignQuesId);
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Assignment",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "AssignmentId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_LC_Audio",
                        column: x => x.Ques_Id,
                        principalTable: "Ques_LC_Audio",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_LC_Conversation",
                        column: x => x.Ques_Id,
                        principalTable: "Ques_LC_Conversation",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_LC_Image",
                        column: x => x.Ques_Id,
                        principalTable: "Ques_LC_Image",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_RC_Double",
                        column: x => x.Ques_Id,
                        principalTable: "Ques_RC_Double",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_RC_Single",
                        column: x => x.Ques_Id,
                        principalTable: "Ques_RC_Single",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_RC_Triple",
                        column: x => x.Ques_Id,
                        principalTable: "Ques_RC_Triple",
                        principalColumn: "QuesId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques",
                table: "Assign_Ques",
                column: "Ques_Id",
                unique: true,
                filter: "[Ques_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_AssignmentId",
                table: "Assign_Ques",
                column: "AssignmentId");
        }
    }
}
