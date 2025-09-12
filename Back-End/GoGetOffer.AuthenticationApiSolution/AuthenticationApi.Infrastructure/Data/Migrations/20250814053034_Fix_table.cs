using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierProfilesId",
                table: "SupplierJoinRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestedAt",
                table: "SupplierJoinRequests",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "IsApproved",
                table: "SupplierJoinRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AdminComment",
                table: "SupplierJoinRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierProfilesId",
                table: "SupplierJoinRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestedAt",
                table: "SupplierJoinRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "IsApproved",
                table: "SupplierJoinRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AdminComment",
                table: "SupplierJoinRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                table: "SupplierJoinRequests",
                column: "SupplierProfilesId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
