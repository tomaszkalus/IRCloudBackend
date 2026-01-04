using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRCloudBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenamedUserProfileToDomainUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUserProfile");

            migrationBuilder.CreateTable(
                name: "DomainUserPost",
                columns: table => new
                {
                    SavedPostsId = table.Column<int>(type: "integer", nullable: false),
                    SavingUsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainUserPost", x => new { x.SavedPostsId, x.SavingUsersId });
                    table.ForeignKey(
                        name: "FK_DomainUserPost_Posts_SavedPostsId",
                        column: x => x.SavedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainUserPost_UserProfiles_SavingUsersId",
                        column: x => x.SavingUsersId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainUserPost_SavingUsersId",
                table: "DomainUserPost",
                column: "SavingUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainUserPost");

            migrationBuilder.CreateTable(
                name: "PostUserProfile",
                columns: table => new
                {
                    SavedPostsId = table.Column<int>(type: "integer", nullable: false),
                    SavingUsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUserProfile", x => new { x.SavedPostsId, x.SavingUsersId });
                    table.ForeignKey(
                        name: "FK_PostUserProfile_Posts_SavedPostsId",
                        column: x => x.SavedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUserProfile_UserProfiles_SavingUsersId",
                        column: x => x.SavingUsersId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUserProfile_SavingUsersId",
                table: "PostUserProfile",
                column: "SavingUsersId");
        }
    }
}
