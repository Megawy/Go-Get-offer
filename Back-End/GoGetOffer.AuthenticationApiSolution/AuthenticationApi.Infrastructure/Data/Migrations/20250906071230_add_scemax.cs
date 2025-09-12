using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_scemax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AuthenticationUser_UserId",
                schema: "Auth",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_UserId",
                schema: "Auth",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Auth",
                table: "Suppliers");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AuthenticationUser_Id",
                schema: "Auth",
                table: "Suppliers",
                column: "Id",
                principalSchema: "Auth",
                principalTable: "AuthenticationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AuthenticationUser_Id",
                schema: "Auth",
                table: "Suppliers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Auth",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_UserId",
                schema: "Auth",
                table: "Suppliers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AuthenticationUser_UserId",
                schema: "Auth",
                table: "Suppliers",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "AuthenticationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
