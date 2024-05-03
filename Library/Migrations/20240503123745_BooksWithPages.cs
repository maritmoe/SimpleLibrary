using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class BooksWithPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 1,
                column: "pages",
                value: 100);

            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 2,
                column: "pages",
                value: 210);

            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 3,
                column: "pages",
                value: 350);

            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 4,
                column: "pages",
                value: 58);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 1,
                column: "pages",
                value: null);

            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 2,
                column: "pages",
                value: null);

            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 3,
                column: "pages",
                value: null);

            migrationBuilder.UpdateData(
                table: "books",
                keyColumn: "id",
                keyValue: 4,
                column: "pages",
                value: null);
        }
    }
}
