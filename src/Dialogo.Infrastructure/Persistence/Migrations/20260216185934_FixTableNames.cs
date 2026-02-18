using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dialogo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_FromUserId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_Users_ToUserId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Users_FriendUserId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Users_UserId",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendRequest",
                table: "FriendRequest");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "Friendships");

            migrationBuilder.RenameTable(
                name: "FriendRequest",
                newName: "FriendRequests");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_UserId_FriendUserId",
                table: "Friendships",
                newName: "IX_Friendships_UserId_FriendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_FriendUserId",
                table: "Friendships",
                newName: "IX_Friendships_FriendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequest_ToUserId",
                table: "FriendRequests",
                newName: "IX_FriendRequests_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequest_FromUserId_ToUserId",
                table: "FriendRequests",
                newName: "IX_FriendRequests_FromUserId_ToUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendRequests",
                table: "FriendRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_FromUserId",
                table: "FriendRequests",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_ToUserId",
                table: "FriendRequests",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_FriendUserId",
                table: "Friendships",
                column: "FriendUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_FromUserId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_ToUserId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_FriendUserId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendRequests",
                table: "FriendRequests");

            migrationBuilder.RenameTable(
                name: "Friendships",
                newName: "Friendship");

            migrationBuilder.RenameTable(
                name: "FriendRequests",
                newName: "FriendRequest");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_UserId_FriendUserId",
                table: "Friendship",
                newName: "IX_Friendship_UserId_FriendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FriendUserId",
                table: "Friendship",
                newName: "IX_Friendship_FriendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequests_ToUserId",
                table: "FriendRequest",
                newName: "IX_FriendRequest_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequests_FromUserId_ToUserId",
                table: "FriendRequest",
                newName: "IX_FriendRequest_FromUserId_ToUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendRequest",
                table: "FriendRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_FromUserId",
                table: "FriendRequest",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_Users_ToUserId",
                table: "FriendRequest",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Users_FriendUserId",
                table: "Friendship",
                column: "FriendUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Users_UserId",
                table: "Friendship",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
