using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleIKnow.Migrations
{
    /// <inheritdoc />
    public partial class AddBirthdayNotificationToggle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendBirthdayNotification",
                table: "Contacts",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendBirthdayNotification",
                table: "Contacts");
        }
    }
}
