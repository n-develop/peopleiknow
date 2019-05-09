using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeopleIKnow.Migrations
{
    public partial class AddedImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Firstname = table.Column<string>(nullable: true),
                    Middlename = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Employer = table.Column<string>(nullable: true),
                    BusinessTitle = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Contacts", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "EmailAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAddresses_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Person = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationships_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusText = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusUpdates_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelephoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Telephone = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelephoneNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelephoneNumbers_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddresses_ContactId",
                table: "EmailAddresses",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_ContactId",
                table: "Relationships",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusUpdates_ContactId",
                table: "StatusUpdates",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_TelephoneNumbers_ContactId",
                table: "TelephoneNumbers",
                column: "ContactId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAddresses");

            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropTable(
                name: "StatusUpdates");

            migrationBuilder.DropTable(
                name: "TelephoneNumbers");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}