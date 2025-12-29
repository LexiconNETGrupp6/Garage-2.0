using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 43, 12, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(2025, 12, 29, 11, 42, 12, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(2025, 12, 29, 12, 25, 23, 0, DateTimeKind.Unspecified), 5 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(2025, 12, 29, 5, 5, 5, 0, DateTimeKind.Unspecified), 6 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 43, 12, 0, DateTimeKind.Unspecified), 8 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(2025, 12, 28, 3, 43, 11, 0, DateTimeKind.Unspecified), 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ArrivalTime", "ParkingSpot" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });
        }
    }
}
