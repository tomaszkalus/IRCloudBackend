using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRCloudBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameToDomainUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "UserProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "UserProfiles");
        }
    }
}
