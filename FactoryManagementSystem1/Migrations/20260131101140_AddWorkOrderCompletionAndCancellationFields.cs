using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryManagementSystem1.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkOrderCompletionAndCancellationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAtUtc",
                table: "WorkOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledByUserId",
                table: "WorkOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAtUtc",
                table: "WorkOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletedByUserId",
                table: "WorkOrders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CancelledByUserId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CompletedAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CompletedByUserId",
                table: "WorkOrders");
        }
    }
}
