using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaAPI.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeFieldsModifiedCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookSpecialTags",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookSpecialTags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookSeries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookSeries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookPublishers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookPublishers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookGenres",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookGenres",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookCopies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookCopies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BookAuthors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "BookAuthors",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookTypes");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookTypes");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookSpecialTags");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookSpecialTags");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookSeries");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookSeries");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookPublishers");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookPublishers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookGenres");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookGenres");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BookAuthors");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "BookAuthors");
        }
    }
}
