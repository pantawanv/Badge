using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Badge.Migrations
{
    /// <inheritdoc />
    public partial class AchievementIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AchievementIcon",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AchievementIcon",
                table: "Achievements");
        }
    }
}
