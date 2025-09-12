using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_xxx1x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuthenticationUserUpdateRequests_AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropColumn(
                name: "AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationUserUpdateRequests_AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId1",
                principalTable: "AuthenticationUser",
                principalColumn: "Id");
        }
    }
}
