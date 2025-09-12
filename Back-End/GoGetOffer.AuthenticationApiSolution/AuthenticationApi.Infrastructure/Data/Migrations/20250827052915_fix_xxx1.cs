using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_xxx1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_UserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuthenticationUserUpdateRequests_UserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId",
                principalTable: "AuthenticationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId1",
                principalTable: "AuthenticationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuthenticationUserUpdateRequests_AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropColumn(
                name: "AuthenticationUserId1",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AuthenticationUserUpdateRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationUserUpdateRequests_UserId",
                table: "AuthenticationUserUpdateRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId",
                principalTable: "AuthenticationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_UserId",
                table: "AuthenticationUserUpdateRequests",
                column: "UserId",
                principalTable: "AuthenticationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
