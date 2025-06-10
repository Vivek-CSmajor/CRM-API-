using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MockCRM.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialDataMadeByCopilot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice@gmail.com", "hash1", "alice" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob@gmail.com", "hash2", 1, "bob" },
                    { 3, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "carol@gmail.com", "hash3", 2, "carol" },
                    { 4, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dave@gmail.com", "hash4", 3, "dave" },
                    { 5, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "eve@gmail.com", "hash5", 2, "eve" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "ID", "AssignedSalesRepId", "Company", "CreatedDate", "Email", "LastContactDate", "Name", "Phone", "Priority", "Revenue", "Status" },
                values: new object[,]
                {
                    { 1, 3, "Acme", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "contact.acme@gmail.com", new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Acme Corp", "1234567890", 0, 0, 0 },
                    { 2, 5, "Beta", new DateTime(2024, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "info.beta@gmail.com", new DateTime(2024, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beta LLC", "2345678901", 1, 0, 2 },
                    { 3, 3, "Gamma", new DateTime(2024, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "hello.gamma@gmail.com", new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gamma Inc", "3456789012", 2, 0, 1 },
                    { 4, 5, "Delta", new DateTime(2024, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "support.delta@gmail.com", new DateTime(2024, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delta Ltd", "4567890123", 1, 0, 0 },
                    { 5, 2, "Epsilon", new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "contact.epsilon@gmail.com", new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Epsilon GmbH", "5678901234", 0, 0, 2 }
                });

            migrationBuilder.InsertData(
                table: "ContactHistories",
                columns: new[] { "ContactHistoryID", "ContactDate", "ContactMethod", "ContactType", "CreatedByUserId", "CustomerID", "Duration", "FollowUpDate", "Notes", "Outcome" },
                values: new object[,]
                {
                    { 10, new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Email", 2, 1, 0, null, "Initial contact", "Interested" },
                    { 20, new DateTime(2024, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Phone", 3, 2, 0, null, "Follow-up call", "No answer" },
                    { 30, new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Meeting", 5, 3, 0, null, "Demo presented", "Considering" },
                    { 40, new DateTime(2024, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Email", 3, 4, 0, null, "Sent proposal", "Waiting" },
                    { 50, new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Phone", 2, 5, 0, null, "Negotiation", "Deal closed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContactHistories",
                keyColumn: "ContactHistoryID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ContactHistories",
                keyColumn: "ContactHistoryID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ContactHistories",
                keyColumn: "ContactHistoryID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ContactHistories",
                keyColumn: "ContactHistoryID",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ContactHistories",
                keyColumn: "ContactHistoryID",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
