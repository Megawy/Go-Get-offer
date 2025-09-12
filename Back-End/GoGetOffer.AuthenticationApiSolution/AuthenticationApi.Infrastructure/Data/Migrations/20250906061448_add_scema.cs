using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_scema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "Suppliers",
                newSchema: "Auth");

            migrationBuilder.RenameTable(
                name: "SupplierJoinRequests",
                newName: "SupplierJoinRequests",
                newSchema: "Auth");

            migrationBuilder.RenameTable(
                name: "SupplierBranches",
                newName: "SupplierBranches",
                newSchema: "Auth");

            migrationBuilder.RenameTable(
                name: "SuppilerProfileUpdates",
                newName: "SuppilerProfileUpdates",
                newSchema: "Auth");

            migrationBuilder.RenameTable(
                name: "AuthenticationUserUpdateRequests",
                newName: "AuthenticationUserUpdateRequests",
                newSchema: "Auth");

            migrationBuilder.RenameTable(
                name: "AuthenticationUser",
                newName: "AuthenticationUser",
                newSchema: "Auth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Suppliers",
                schema: "Auth",
                newName: "Suppliers");

            migrationBuilder.RenameTable(
                name: "SupplierJoinRequests",
                schema: "Auth",
                newName: "SupplierJoinRequests");

            migrationBuilder.RenameTable(
                name: "SupplierBranches",
                schema: "Auth",
                newName: "SupplierBranches");

            migrationBuilder.RenameTable(
                name: "SuppilerProfileUpdates",
                schema: "Auth",
                newName: "SuppilerProfileUpdates");

            migrationBuilder.RenameTable(
                name: "AuthenticationUserUpdateRequests",
                schema: "Auth",
                newName: "AuthenticationUserUpdateRequests");

            migrationBuilder.RenameTable(
                name: "AuthenticationUser",
                schema: "Auth",
                newName: "AuthenticationUser");
        }
    }
}
