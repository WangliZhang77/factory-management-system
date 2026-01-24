using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryManagementSystem1.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkOrderApprovalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAtUtc",
                table: "WorkOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserId",
                table: "WorkOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAtUtc",
                table: "WorkOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedByUserId",
                table: "WorkOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "WorkOrders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RejectedAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RejectedByUserId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "WorkOrders");
        }
    }
}
