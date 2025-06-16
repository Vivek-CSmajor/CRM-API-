using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockCRM.Migrations
{
    /// <inheritdoc />
    public partial class TestingCRMDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "ContactHistories",
                columns: new[] { "ContactHistoryID", "ContactDate", "ContactMethod", "ContactType", "CreatedByUserId", "CustomerID", "Duration", "FollowUpDate", "Notes", "Outcome" },
                values: new object[] { 110, new DateTime(2025, 6, 15, 20, 0, 0, 0, DateTimeKind.Unspecified), 0, "Phone", 2, 1, 10, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scheduled follow-up for testing", "Pending" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContactHistories",
                keyColumn: "ContactHistoryID",
                keyValue: 110);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
