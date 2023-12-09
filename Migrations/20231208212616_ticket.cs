using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Badge.Migrations
{
    /// <inheritdoc />
    public partial class ticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketGroupAssigns_Group_GroupId",
                table: "TicketGroupAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketGroupAssigns_Ticket_TicketId",
                table: "TicketGroupAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketMemberAssigns_Member_MemberId",
                table: "TicketMemberAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketMemberAssigns_Ticket_TicketId",
                table: "TicketMemberAssigns");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMemberAssigns_TicketId",
                table: "TicketMemberAssigns",
                column: "TicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketGroupAssigns_TicketId",
                table: "TicketGroupAssigns",
                column: "TicketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketGroupAssigns_Group_GroupId",
                table: "TicketGroupAssigns",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketGroupAssigns_Ticket_TicketId",
                table: "TicketGroupAssigns",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMemberAssigns_Member_MemberId",
                table: "TicketMemberAssigns",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMemberAssigns_Ticket_TicketId",
                table: "TicketMemberAssigns",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketGroupAssigns_Group_GroupId",
                table: "TicketGroupAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketGroupAssigns_Ticket_TicketId",
                table: "TicketGroupAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketMemberAssigns_Member_MemberId",
                table: "TicketMemberAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketMemberAssigns_Ticket_TicketId",
                table: "TicketMemberAssigns");

            migrationBuilder.DropIndex(
                name: "IX_TicketMemberAssigns_TicketId",
                table: "TicketMemberAssigns");

            migrationBuilder.DropIndex(
                name: "IX_TicketGroupAssigns_TicketId",
                table: "TicketGroupAssigns");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketGroupAssigns_Group_GroupId",
                table: "TicketGroupAssigns",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketGroupAssigns_Ticket_TicketId",
                table: "TicketGroupAssigns",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMemberAssigns_Member_MemberId",
                table: "TicketMemberAssigns",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMemberAssigns_Ticket_TicketId",
                table: "TicketMemberAssigns",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");
        }
    }
}
