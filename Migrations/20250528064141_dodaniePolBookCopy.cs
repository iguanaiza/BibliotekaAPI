using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaAPI.Migrations
{
    /// <inheritdoc />
    public partial class dodaniePolBookCopy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InventoryNum",
                table: "BookCopies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "BookCopies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryNum",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "BookCopies");
        }
    }
}
