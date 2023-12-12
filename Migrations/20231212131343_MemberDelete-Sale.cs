using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Badge.Migrations
{
    /// <inheritdoc />
    public partial class MemberDeleteSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Ticket_TicketId",
                table: "Sale");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Ticket_TicketId",
                table: "Sale",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Ticket_TicketId",
                table: "Sale");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Ticket_TicketId",
                table: "Sale",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
