using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCenter.Migrations
{
    /// <inheritdoc />
    public partial class addTblHomeQue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Home_Ques",
                columns: table => new
                {
                    HomeQuesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    NoNum = table.Column<int>(type: "int", nullable: false),
                    ImageQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    AudioQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    ConversationQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    SingleQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    DoubleQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    TripleQues_Id = table.Column<long>(type: "bigint", nullable: true),
                    SentenceQuesId = table.Column<long>(type: "bigint", nullable: true),
                    HomeworkId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Home_Ques", x => x.HomeQuesId);
                    table.ForeignKey(
                        name: "FK_Home_Ques_Homework",
                        column: x => x.HomeworkId,
                        principalTable: "Homework",
                        principalColumn: "HomeworkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Home_Ques_Ques_LC_Audio",
                        column: x => x.AudioQues_Id,
                        principalTable: "Ques_LC_Audio",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Home_Ques_Ques_LC_Conversation",
                        column: x => x.ConversationQues_Id,
                        principalTable: "Ques_LC_Conversation",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Home_Ques_Ques_LC_Image",
                        column: x => x.ImageQues_Id,
                        principalTable: "Ques_LC_Image",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Home_Ques_Ques_RC_Double",
                        column: x => x.DoubleQues_Id,
                        principalTable: "Ques_RC_Double",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Home_Ques_Ques_RC_Sentence",
                        column: x => x.SentenceQuesId,
                        principalTable: "Ques_RC_Sentence",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Home_Ques_Ques_RC_Single",
                        column: x => x.SingleQues_Id,
                        principalTable: "Ques_RC_Single",
                        principalColumn: "QuesId");
                    table.ForeignKey(
                        name: "FK_Home_Ques_Ques_RC_Triple",
                        column: x => x.TripleQues_Id,
                        principalTable: "Ques_RC_Triple",
                        principalColumn: "QuesId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HW_Sub_Records_HwQuesId",
                table: "HW_Sub_Records",
                column: "HwQuesId");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_AudioQues_Id",
                table: "Home_Ques",
                column: "AudioQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_ConversationQues_Id",
                table: "Home_Ques",
                column: "ConversationQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_DoubleQues_Id",
                table: "Home_Ques",
                column: "DoubleQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_HomeworkId",
                table: "Home_Ques",
                column: "HomeworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_ImageQues_Id",
                table: "Home_Ques",
                column: "ImageQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_SentenceQuesId",
                table: "Home_Ques",
                column: "SentenceQuesId");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_SingleQues_Id",
                table: "Home_Ques",
                column: "SingleQues_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Home_Ques_TripleQues_Id",
                table: "Home_Ques",
                column: "TripleQues_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hw_Sub_Record_HomeQue",
                table: "HW_Sub_Records",
                column: "HwQuesId",
                principalTable: "Home_Ques",
                principalColumn: "HomeQuesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hw_Sub_Record_HomeQue",
                table: "HW_Sub_Records");

            migrationBuilder.DropTable(
                name: "Home_Ques");

            migrationBuilder.DropIndex(
                name: "IX_HW_Sub_Records_HwQuesId",
                table: "HW_Sub_Records");
        }
    }
}
