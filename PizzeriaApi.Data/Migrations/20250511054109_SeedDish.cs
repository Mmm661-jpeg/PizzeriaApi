using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 1, 1, "Placeholder for removed dishes", "Deleted Dish", 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
