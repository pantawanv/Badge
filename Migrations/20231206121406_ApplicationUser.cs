using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Badge.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUser : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "AppUImageData",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketGroupAssigns_Group_GroupId",
                table: "TicketGroupAssigns",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketGroupAssigns_Ticket_TicketId",
                table: "TicketGroupAssigns",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMemberAssigns_Member_MemberId",
                table: "TicketMemberAssigns",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMemberAssigns_Ticket_TicketId",
                table: "TicketMemberAssigns",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropColumn(
                name: "AppUImageData",
                table: "AspNetUsers");

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
