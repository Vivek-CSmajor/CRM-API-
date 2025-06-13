using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MockCRM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Revenue = table.Column<int>(type: "int", nullable: true),
                    AssignedSalesRepId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastContactDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customers_Users_AssignedSalesRepId",
                        column: x => x.AssignedSalesRepId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactHistories",
                columns: table => new
                {
                    ContactHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    ContactType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ContactMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactHistories", x => x.ContactHistoryID);
                    table.ForeignKey(
                        name: "FK_ContactHistories_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactHistories_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "ID", "AssignedSalesRepId", "Company", "CreatedDate", "Email", "LastContactDate", "Name", "Phone", "Priority", "Revenue", "Status" },
                values: new object[] { 4, null, "Delta", new DateTime(2024, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "support.delta@gmail.com", null, "Delta Ltd", null, 1, 75000, 0 });

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
                table: "ContactHistories",
                columns: new[] { "ContactHistoryID", "ContactDate", "ContactMethod", "ContactType", "CreatedByUserId", "CustomerID", "Duration", "FollowUpDate", "Notes", "Outcome" },
                values: new object[,]
                {
                    { 40, new DateTime(2024, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "VideoCall", 3, 4, 10, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sent proposal", "Waiting" },
                    { 90, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Phone", 2, 4, 25, null, "Contract renewal", "Renewed" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "ID", "AssignedSalesRepId", "Company", "CreatedDate", "Email", "LastContactDate", "Name", "Phone", "Priority", "Revenue", "Status" },
                values: new object[,]
                {
                    { 1, 3, "Acme", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "contact.acme@gmail.com", new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Acme Corp", "+91 9123456789", 0, 100000, 0 },
                    { 2, 5, "Beta", new DateTime(2024, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "info.beta@gmail.com", new DateTime(2024, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beta LLC", "+91 9234567890", 1, 50000, 2 },
                    { 3, 3, "Gamma", new DateTime(2024, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "hello.gamma@gmail.com", new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gamma Inc", "+91 9345678901", 2, null, 1 },
                    { 5, 2, null, new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "contact.epsilon@gmail.com", new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Epsilon GmbH", "+91 9456789012", 0, 120000, 2 }
                });

            migrationBuilder.InsertData(
                table: "ContactHistories",
                columns: new[] { "ContactHistoryID", "ContactDate", "ContactMethod", "ContactType", "CreatedByUserId", "CustomerID", "Duration", "FollowUpDate", "Notes", "Outcome" },
                values: new object[,]
                {
                    { 10, new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Email", 2, 1, 30, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Initial contact", "Interested" },
                    { 20, new DateTime(2024, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Phone", 3, 2, 15, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Follow-up call", "No answer" },
                    { 30, new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Meeting", 5, 3, 60, null, "Demo presented", "Considering" },
                    { 50, new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Phone", 2, 5, 45, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Negotiation", "Deal closed" },
                    { 60, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Email", 1, 1, 5, new DateTime(2024, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Follow-up overdue", "No response" },
                    { 70, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "InPerson", 4, 2, 90, null, "On-site visit", "Demo scheduled" },
                    { 80, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "VideoCall", 5, 3, 20, new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Remote support", "Resolved" },
                    { 100, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Email", 1, 5, 10, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Feedback requested", "Positive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactHistories_CreatedByUserId",
                table: "ContactHistories",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactHistories_CustomerID",
                table: "ContactHistories",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AssignedSalesRepId",
                table: "Customers",
                column: "AssignedSalesRepId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactHistories");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
