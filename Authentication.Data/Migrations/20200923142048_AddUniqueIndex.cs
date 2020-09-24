using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Data.Migrations
{
    public partial class AddUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionType",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId_PermissionType",
                table: "RolePermissions");
        }
    }
}
