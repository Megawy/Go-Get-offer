using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_xxx1xzxzz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId",
                principalTable: "AuthenticationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_AuthenticationUserId",
                table: "AuthenticationUserUpdateRequests",
                column: "AuthenticationUserId",
                principalTable: "AuthenticationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
