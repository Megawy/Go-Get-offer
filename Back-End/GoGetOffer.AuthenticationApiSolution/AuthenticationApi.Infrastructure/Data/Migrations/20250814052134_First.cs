using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenticationUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsStatusConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationUser", x => x.Id);
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
                name: "ClientProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    BranchCount = table.Column<int>(type: "int", nullable: false),
                    CommercialRegistrationDocumentUrl = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false),
                    TaxCardDocumentUrl = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false),
                    TOS = table.Column<bool>(type: "bit", nullable: false)
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
                name: "AuthenticationUserUpdateRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<int>(type: "int", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationUserUpdateRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthenticationUserUpdateRequests_AuthenticationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthenticationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    CommercialRegistrationDocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxCardDocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinProducts = table.Column<int>(type: "int", nullable: true),
                    MaxProducts = table.Column<int>(type: "int", nullable: true),
                    DeliveryTimeInDays = table.Column<int>(type: "int", nullable: true),
                    MinInvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HasElctroinInvoice = table.Column<bool>(type: "bit", nullable: false),
                    HasDeliveryService = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_AuthenticationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthenticationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GovernorateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
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
                    RecipientName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
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
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "ProfileSuppilerUpdates",
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
                    IsApproved = table.Column<int>(type: "int", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupplierProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileSuppilerUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileSuppilerUpdates_Suppliers_SupplierProfileId",
                        column: x => x.SupplierProfileId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierJoinRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<int>(type: "int", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupplierProfilesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierJoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierJoinRequests_Suppliers_SupplierProfilesId",
                        column: x => x.SupplierProfilesId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "IX_AuthenticationUser_CompanyName",
                table: "AuthenticationUser",
                column: "CompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationUser_Email",
                table: "AuthenticationUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationUser_PhoneNumber",
                table: "AuthenticationUser",
                column: "PhoneNumber",
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
                name: "IX_AuthenticationUserUpdateRequests_UserId",
                table: "AuthenticationUserUpdateRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSuppilerUpdates_SupplierProfileId",
                table: "ProfileSuppilerUpdates",
                column: "SupplierProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddresses_ClientProfileId",
                table: "ShippingAddresses",
                column: "ClientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SubClients_ClientProfileId",
                table: "SubClients",
                column: "ClientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierJoinRequests_SupplierProfilesId",
                table: "SupplierJoinRequests",
                column: "SupplierProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_UserId",
                table: "Suppliers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "AuthenticationUserUpdateRequests");

            migrationBuilder.DropTable(
                name: "ProfileSuppilerUpdates");

            migrationBuilder.DropTable(
                name: "ShippingAddresses");

            migrationBuilder.DropTable(
                name: "SubClients");

            migrationBuilder.DropTable(
                name: "SupplierJoinRequests");

            migrationBuilder.DropTable(
                name: "Citys");

            migrationBuilder.DropTable(
                name: "ClientProfiles");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropTable(
                name: "AuthenticationUser");
        }
    }
}
