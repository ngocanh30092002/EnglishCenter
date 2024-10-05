using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addAssignQues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assign_Ques",
                columns: table => new
                {
                    AssignQuesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ImageQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    AudioQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    ConversationQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    SingleQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    DoubleQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    TripleQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    AssignmentId = table.Column<long>(type: "bigint", nullable: true)
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
                        column: x => x.AudioQues_Id,
                        principalTable: "Ques_LC_Audio",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_LC_Conversation",
                        column: x => x.ConversationQues_Id,
                        principalTable: "Ques_LC_Conversation",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_LC_Image",
                        column: x => x.ImageQues_Id,
                        principalTable: "Ques_LC_Image",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_RC_Double",
                        column: x => x.DoubleQues_Id,
                        principalTable: "Ques_RC_Double",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_RC_Single",
                        column: x => x.SingleQues_Id,
                        principalTable: "Ques_RC_Single",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Assign_Ques_Ques_RC_Triple",
                        column: x => x.TripleQues_Id,
                        principalTable: "Ques_RC_Triple",
                        principalColumn: "QuesId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_AssignmentId",
                table: "Assign_Ques",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_AudioQues_Id",
                table: "Assign_Ques",
                column: "AudioQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_ConversationQues_Id",
                table: "Assign_Ques",
                column: "ConversationQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_DoubleQues_Id",
                table: "Assign_Ques",
                column: "DoubleQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_ImageQues_Id",
                table: "Assign_Ques",
                column: "ImageQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_SingleQues_Id",
                table: "Assign_Ques",
                column: "SingleQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_Ques_TripleQues_Id",
                table: "Assign_Ques",
                column: "TripleQues_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assign_Ques");
        }
    }
}
