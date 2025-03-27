using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpLN.Migrations
{
    /// <inheritdoc />
    public partial class UserNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNote",
                table: "Payment",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNote",
                table: "Payment");
        }
    }
}
