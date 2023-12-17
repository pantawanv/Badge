using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Badge.Migrations
{
    /// <inheritdoc />
    public partial class ParentEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parent_Member_MemberId",
                table: "Parent");

            migrationBuilder.DropIndex(
                name: "IX_Parent_MemberId",
                table: "Parent");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Parent");

            migrationBuilder.CreateTable(
                name: "MemberParents",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberParents", x => new { x.MemberId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_MemberParents_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberParents_Parent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberParents_ParentId",
                table: "MemberParents",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberParents");

            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Parent",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Parent_MemberId",
                table: "Parent",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parent_Member_MemberId",
                table: "Parent",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
