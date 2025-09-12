using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationUserUpdateRequests_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId",
                principalTable: "AuthenticationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuthenticationUserUpdateRequests_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropColumn(
                name: "AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests");
        }
    }
}
