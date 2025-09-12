using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_xxx1xzxzzx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuppilerProfileUpdates_Suppliers_SupplierProfilesId",
                table: "SuppilerProfileUpdates");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierBranches_Suppliers_SupplierProfilesId",
                table: "SupplierBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_SuppilerProfileUpdates_Suppliers_SupplierProfilesId",
                table: "SuppilerProfileUpdates",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierBranches_Suppliers_SupplierProfilesId",
                table: "SupplierBranches",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuppilerProfileUpdates_Suppliers_SupplierProfilesId",
                table: "SuppilerProfileUpdates");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierBranches_Suppliers_SupplierProfilesId",
                table: "SupplierBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_SuppilerProfileUpdates_Suppliers_SupplierProfilesId",
                table: "SuppilerProfileUpdates",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierBranches_Suppliers_SupplierProfilesId",
                table: "SupplierBranches",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
