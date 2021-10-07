using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagement.Data.Migrations
{
    public partial class AssignAdminUserToAllRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[UserRoles] (UserId, RoleId) SELECT '6fc8b2c8-05ec-4db6-ae06-1d755ec3b948', Id FROM [security].[Roles] ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[UserRoles] WHERE UserId = '6fc8b2c8-05ec-4db6-ae06-1d755ec3b948'");
        }
    }
}
