using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRCloudBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenamedUserProfileToDomainUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainUserPost_UserProfiles_SavingUsersId",
                table: "DomainUserPost");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserProfiles_AuthorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_UserProfiles_AuthorId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_UserProfiles_FollowedId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_UserProfiles_FollowerId",
                table: "UserFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "DomainUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DomainUsers",
                table: "DomainUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainUserPost_DomainUsers_SavingUsersId",
                table: "DomainUserPost",
                column: "SavingUsersId",
                principalTable: "DomainUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_DomainUsers_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "DomainUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_DomainUsers_AuthorId",
                table: "Reviews",
                column: "AuthorId",
                principalTable: "DomainUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_DomainUsers_FollowedId",
                table: "UserFollows",
                column: "FollowedId",
                principalTable: "DomainUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_DomainUsers_FollowerId",
                table: "UserFollows",
                column: "FollowerId",
                principalTable: "DomainUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainUserPost_DomainUsers_SavingUsersId",
                table: "DomainUserPost");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_DomainUsers_AuthorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_DomainUsers_AuthorId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_DomainUsers_FollowedId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_DomainUsers_FollowerId",
                table: "UserFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DomainUsers",
                table: "DomainUsers");

            migrationBuilder.RenameTable(
                name: "DomainUsers",
                newName: "UserProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainUserPost_UserProfiles_SavingUsersId",
                table: "DomainUserPost",
                column: "SavingUsersId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserProfiles_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_UserProfiles_AuthorId",
                table: "Reviews",
                column: "AuthorId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_UserProfiles_FollowedId",
                table: "UserFollows",
                column: "FollowedId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_UserProfiles_FollowerId",
                table: "UserFollows",
                column: "FollowerId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
