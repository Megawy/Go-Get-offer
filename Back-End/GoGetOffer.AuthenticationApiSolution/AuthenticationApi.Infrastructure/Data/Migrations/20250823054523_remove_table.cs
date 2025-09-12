using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class remove_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "ProfileSuppilerUpdates");

            migrationBuilder.DropTable(
                name: "ShippingAddresses");

            migrationBuilder.DropTable(
                name: "SubClients");

            migrationBuilder.DropTable(
                name: "Citys");

            migrationBuilder.DropTable(
                name: "ClientProfiles");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.CreateTable(
                name: "SuppilerProfileUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewGovernorate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewAddressDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupplierProfilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppilerProfileUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuppilerProfileUpdates_Suppliers_SupplierProfilesId",
                        column: x => x.SupplierProfilesId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuppilerProfileUpdates_SupplierProfilesId",
                table: "SuppilerProfileUpdates",
                column: "SupplierProfilesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuppilerProfileUpdates");

            migrationBuilder.CreateTable(
                name: "ClientProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchCount = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommercialRegistrationDocumentUrl = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOS = table.Column<bool>(type: "bit", nullable: false),
                    TaxCardDocumentUrl = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProfiles_AuthenticationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthenticationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileSuppilerUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierProfilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NewAddressDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewGovernorate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileSuppilerUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileSuppilerUpdates_Suppliers_SupplierProfilesId",
                        column: x => x.SupplierProfilesId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_ClientProfiles_ClientProfileId",
                        column: x => x.ClientProfileId,
                        principalTable: "ClientProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    RecipientName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingAddresses_ClientProfiles_ClientProfileId",
                        column: x => x.ClientProfileId,
                        principalTable: "ClientProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubClients_ClientProfiles_ClientProfileId",
                        column: x => x.ClientProfileId,
                        principalTable: "ClientProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovernorateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citys_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_Citys_CityId",
                        column: x => x.CityId,
                        principalTable: "Citys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Area_CityId",
                table: "Area",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_Name",
                table: "Area",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ClientProfileId",
                table: "Branches",
                column: "ClientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Citys_GovernorateId",
                table: "Citys",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Citys_Name",
                table: "Citys",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfiles_UserId",
                table: "ClientProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_Name",
                table: "Governorates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSuppilerUpdates_SupplierProfilesId",
                table: "ProfileSuppilerUpdates",
                column: "SupplierProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddresses_ClientProfileId",
                table: "ShippingAddresses",
                column: "ClientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SubClients_ClientProfileId",
                table: "SubClients",
                column: "ClientProfileId");
        }
    }
}
