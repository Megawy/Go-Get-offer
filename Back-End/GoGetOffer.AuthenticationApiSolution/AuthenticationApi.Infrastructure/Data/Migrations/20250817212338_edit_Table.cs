using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class edit_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressDetails",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PhoneNumbers",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Suppliers");

            migrationBuilder.CreateTable(
                name: "SupplierBranches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierProfilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierBranches_Suppliers_SupplierProfilesId",
                        column: x => x.SupplierProfilesId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierBranches_SupplierProfilesId",
                table: "SupplierBranches",
                column: "SupplierProfilesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierBranches");

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails",
                table: "Suppliers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Suppliers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Suppliers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "Suppliers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumbers",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
