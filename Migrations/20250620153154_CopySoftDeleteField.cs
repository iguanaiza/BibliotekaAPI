using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaAPI.Migrations
{
    /// <inheritdoc />
    public partial class CopySoftDeleteField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BookCopies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
