using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    pages = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "borrowings",
                columns: table => new
                {
                    borrowed_date = table.Column<DateOnly>(type: "date", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    book_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_borrowings", x => new { x.borrowed_date, x.user_id, x.book_id });
                    table.ForeignKey(
                        name: "FK_borrowings_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_borrowings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "books",
                columns: new[] { "id", "pages", "title" },
                values: new object[,]
                {
                    { 1, null, "C# Book For Beginners" },
                    { 2, null, "React Volume 2" },
                    { 3, null, "Java" },
                    { 4, null, "How To Use Postgres" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "John Doe" },
                    { 2, "Jane Doe" },
                    { 3, "John Smith" },
                    { 4, "Jane Smith" }
                });

            migrationBuilder.InsertData(
                table: "borrowings",
                columns: new[] { "book_id", "borrowed_date", "user_id" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 1, 1), 1 },
                    { 4, new DateOnly(2024, 1, 11), 3 },
                    { 3, new DateOnly(2024, 1, 15), 4 },
                    { 2, new DateOnly(2024, 1, 20), 2 },
                    { 1, new DateOnly(2024, 2, 1), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_borrowings_book_id",
                table: "borrowings",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_borrowings_user_id",
                table: "borrowings",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "borrowings");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
