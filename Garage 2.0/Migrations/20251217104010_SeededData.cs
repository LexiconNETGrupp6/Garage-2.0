using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class SeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "Id", "Brand", "Color", "Model", "NumberOfWheels", "RegNumber", "VehicleType" },
                values: new object[,]
                {
                    { 1, "Volvo", "Red", "V70", 4, "ABC123", 0 },
                    { 2, "East Marine", "Yellow", "Viking Line", 0, "LGH436", 4 },
                    { 3, "Volvo", "Red", "V7900", 8, "AHC745", 2 },
                    { 4, "Teesla", "White", "X", 4, "KAK156", 0 },
                    { 5, "Scania", "Blue", "G-series", 6, "IKA71U", 3 },
                    { 6, "Mercedez", "Green", "L420", 2, "ÅJAUIV", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
