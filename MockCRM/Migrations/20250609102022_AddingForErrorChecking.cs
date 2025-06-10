using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockCRM.Migrations
{
    /// <inheritdoc />
    public partial class AddingForErrorChecking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactMethod",
                table: "ContactHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "ContactHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "ContactHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpDate",
                table: "ContactHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactHistories_CreatedByUserId",
                table: "ContactHistories",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactHistories_Users_CreatedByUserId",
                table: "ContactHistories",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactHistories_Users_CreatedByUserId",
                table: "ContactHistories");

            migrationBuilder.DropIndex(
                name: "IX_ContactHistories_CreatedByUserId",
                table: "ContactHistories");

            migrationBuilder.DropColumn(
                name: "ContactMethod",
                table: "ContactHistories");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ContactHistories");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ContactHistories");

            migrationBuilder.DropColumn(
                name: "FollowUpDate",
                table: "ContactHistories");
        }
    }
}
