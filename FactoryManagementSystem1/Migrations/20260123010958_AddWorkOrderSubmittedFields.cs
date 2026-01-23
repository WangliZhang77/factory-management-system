using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryManagementSystem1.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkOrderSubmittedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAtUtc",
                table: "WorkOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedByUserId",
                table: "WorkOrders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "SubmittedByUserId",
                table: "WorkOrders");
        }
    }
}
