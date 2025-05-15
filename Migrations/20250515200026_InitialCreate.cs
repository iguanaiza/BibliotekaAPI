using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookTypeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTypeEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenreEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublisherEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublisherEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeriesEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isbn = table.Column<int>(type: "int", nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    BookTypeId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookEntries_AuthorEntries_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AuthorEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEntries_BookTypeEntries_TypeId",
                        column: x => x.TypeId,
                        principalTable: "BookTypeEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEntries_CategoryEntries_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEntries_PublisherEntries_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "PublisherEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEntries_SeriesEntries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "SeriesEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookEntryGenreEntry",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEntryGenreEntry", x => new { x.BooksId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookEntryGenreEntry_BookEntries_BooksId",
                        column: x => x.BooksId,
                        principalTable: "BookEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEntryGenreEntry_GenreEntries_GenreId",
                        column: x => x.GenreId,
                        principalTable: "GenreEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CopyEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    BookEntryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CopyEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CopyEntries_BookEntries_BookEntryId",
                        column: x => x.BookEntryId,
                        principalTable: "BookEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookEntries_AuthorId",
                table: "BookEntries",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEntries_CategoryId",
                table: "BookEntries",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEntries_PublisherId",
                table: "BookEntries",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEntries_SeriesId",
                table: "BookEntries",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEntries_TypeId",
                table: "BookEntries",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEntryGenreEntry_GenreId",
                table: "BookEntryGenreEntry",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_CopyEntries_BookEntryId",
                table: "CopyEntries",
                column: "BookEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookEntryGenreEntry");

            migrationBuilder.DropTable(
                name: "CopyEntries");

            migrationBuilder.DropTable(
                name: "GenreEntries");

            migrationBuilder.DropTable(
                name: "BookEntries");

            migrationBuilder.DropTable(
                name: "AuthorEntries");

            migrationBuilder.DropTable(
                name: "BookTypeEntries");

            migrationBuilder.DropTable(
                name: "CategoryEntries");

            migrationBuilder.DropTable(
                name: "PublisherEntries");

            migrationBuilder.DropTable(
                name: "SeriesEntries");
        }
    }
}
