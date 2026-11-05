using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wt_lab6_6minimalapi_cherevan.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssigneeId", "CreatedAt", "Description", "DueDate", "Priority", "ProjectId", "Status", "Title" },
                values: new object[,]
                {
                    { 10, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Прочитать документацию и сделать лабу", new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), "High", 1, "In Progress", "Изучить Entity Framework Core" },
                    { 11, 2, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Установить, настроить и подключить к проекту", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 1, "Done", "Настроить PostgreSQL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
