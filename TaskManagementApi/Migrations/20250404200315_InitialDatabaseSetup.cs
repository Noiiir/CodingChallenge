using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate" },
                values: new object[] { new DateTime(2025, 4, 4, 16, 3, 15, 732, DateTimeKind.Local).AddTicks(838), new DateTime(2025, 4, 11, 16, 3, 15, 732, DateTimeKind.Local).AddTicks(514) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DueDate" },
                values: new object[] { new DateTime(2025, 4, 4, 16, 3, 15, 732, DateTimeKind.Local).AddTicks(940), new DateTime(2025, 4, 18, 16, 3, 15, 732, DateTimeKind.Local).AddTicks(938) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate" },
                values: new object[] { new DateTime(2025, 4, 4, 15, 59, 51, 365, DateTimeKind.Local).AddTicks(3310), new DateTime(2025, 4, 11, 15, 59, 51, 365, DateTimeKind.Local).AddTicks(2981) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DueDate" },
                values: new object[] { new DateTime(2025, 4, 4, 15, 59, 51, 365, DateTimeKind.Local).AddTicks(3399), new DateTime(2025, 4, 18, 15, 59, 51, 365, DateTimeKind.Local).AddTicks(3397) });
        }
    }
}
