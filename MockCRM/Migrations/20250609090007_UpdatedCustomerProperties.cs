using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockCRM.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCustomerProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedSalesRepId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Revenue",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AssignedSalesRepId",
                table: "Customers",
                column: "AssignedSalesRepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_AssignedSalesRepId",
                table: "Customers",
                column: "AssignedSalesRepId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_AssignedSalesRepId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_AssignedSalesRepId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AssignedSalesRepId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customers");
        }
    }
}
