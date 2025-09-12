using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSuppilerUpdates_Suppliers_SupplierProfileId",
                table: "ProfileSuppilerUpdates");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSuppilerUpdates_SupplierProfileId",
                table: "ProfileSuppilerUpdates");

            migrationBuilder.DropColumn(
                name: "SupplierProfileId",
                table: "ProfileSuppilerUpdates");

            migrationBuilder.AlterColumn<int>(
                name: "IsApproved",
                table: "ProfileSuppilerUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierProfilesId",
                table: "ProfileSuppilerUpdates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSuppilerUpdates_SupplierProfilesId",
                table: "ProfileSuppilerUpdates",
                column: "SupplierProfilesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSuppilerUpdates_Suppliers_SupplierProfilesId",
                table: "ProfileSuppilerUpdates",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSuppilerUpdates_Suppliers_SupplierProfilesId",
                table: "ProfileSuppilerUpdates");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSuppilerUpdates_SupplierProfilesId",
                table: "ProfileSuppilerUpdates");

            migrationBuilder.DropColumn(
                name: "SupplierProfilesId",
                table: "ProfileSuppilerUpdates");

            migrationBuilder.AlterColumn<int>(
                name: "IsApproved",
                table: "ProfileSuppilerUpdates",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierProfileId",
                table: "ProfileSuppilerUpdates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSuppilerUpdates_SupplierProfileId",
                table: "ProfileSuppilerUpdates",
                column: "SupplierProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSuppilerUpdates_Suppliers_SupplierProfileId",
                table: "ProfileSuppilerUpdates",
                column: "SupplierProfileId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
