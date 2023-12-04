using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Badge.Migrations
{
    /// <inheritdoc />
    public partial class TicketAssigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "TicketGroupAssigns",
                columns: table => new
                {
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketGroupAssigns", x => new { x.TicketId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_TicketGroupAssigns_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketGroupAssigns_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketMemberAssigns",
                columns: table => new
                {
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMemberAssigns", x => new { x.TicketId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_TicketMemberAssigns_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketMemberAssigns_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketGroupAssigns_GroupId",
                table: "TicketGroupAssigns",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMemberAssigns_MemberId",
                table: "TicketMemberAssigns",
                column: "MemberId");
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "TicketGroupAssigns");

            migrationBuilder.DropTable(
                name: "TicketMemberAssigns");



        }
    }
    
}
