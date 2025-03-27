using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpLN.Migrations
{
    /// <inheritdoc />
    public partial class MoreInfoToHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceString",
                table: "Payment",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceString",
                table: "Payment");
        }
    }
}
