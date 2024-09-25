using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace foodies_api.Migrations
{
    /// <inheritdoc />
    public partial class UserLikeBusinessCorectedNavProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_userLikeBusinesses_UserLikeBusinessId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_userLikeBusinesses_UserLikeBusinessId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "BusinessUser");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserLikeBusinessId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userLikeBusinesses",
                table: "userLikeBusinesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_UserLikeBusinessId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "UserLikeBusinessId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "userLikeBusinesses");

            migrationBuilder.DropColumn(
                name: "UserLikeBusinessId",
                table: "Businesses");

            migrationBuilder.RenameTable(
                name: "userLikeBusinesses",
                newName: "UserLikeBusinesses");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessId",
                table: "UserLikeBusinesses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Businesses",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikeBusinesses",
                table: "UserLikeBusinesses",
                columns: new[] { "UserId", "BusinessId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeBusinesses_BusinessId",
                table: "UserLikeBusinesses",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeBusinesses_Businesses_BusinessId",
                table: "UserLikeBusinesses",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeBusinesses_Users_UserId",
                table: "UserLikeBusinesses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeBusinesses_Businesses_BusinessId",
                table: "UserLikeBusinesses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeBusinesses_Users_UserId",
                table: "UserLikeBusinesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikeBusinesses",
                table: "UserLikeBusinesses");

            migrationBuilder.DropIndex(
                name: "IX_UserLikeBusinesses_BusinessId",
                table: "UserLikeBusinesses");

            migrationBuilder.RenameTable(
                name: "UserLikeBusinesses",
                newName: "userLikeBusinesses");

            migrationBuilder.AddColumn<int>(
                name: "UserLikeBusinessId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessId",
                table: "userLikeBusinesses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "userLikeBusinesses",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Businesses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "UserLikeBusinessId",
                table: "Businesses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userLikeBusinesses",
                table: "userLikeBusinesses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BusinessUser",
                columns: table => new
                {
                    BusinessesId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUser", x => new { x.BusinessesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_BusinessUser_Businesses_BusinessesId",
                        column: x => x.BusinessesId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserLikeBusinessId",
                table: "Users",
                column: "UserLikeBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UserLikeBusinessId",
                table: "Businesses",
                column: "UserLikeBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_UsersId",
                table: "BusinessUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_userLikeBusinesses_UserLikeBusinessId",
                table: "Businesses",
                column: "UserLikeBusinessId",
                principalTable: "userLikeBusinesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_userLikeBusinesses_UserLikeBusinessId",
                table: "Users",
                column: "UserLikeBusinessId",
                principalTable: "userLikeBusinesses",
                principalColumn: "Id");
        }
    }
}
