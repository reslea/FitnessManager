using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Data.Migrations
{
    public partial class RemoveRefreshTokenId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId_PermissionType",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionType",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId_PermissionType",
                table: "RolePermissions");

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionType",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionType" });
        }
    }
}
