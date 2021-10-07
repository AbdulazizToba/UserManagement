using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagement.Data.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) VALUES (N'6fc8b2c8-05ec-4db6-ae06-1d755ec3b948', N'Abdulaziz', N'ABDULAZIZ', N'Abdulaziz@gmail.com', N'ABDULAZIZ@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAECstvjGl/8EotW7h/wqL0xpeayW/WEDQ/d5TZlOlmPweICbcFX8a2r+bds1Uz025ag==', N'ROHV37ZDFHUDWYYYDNQGVPHAGHJBUHDN', N'0adf8445-bee3-4811-b483-f3b660210015', NULL, 0, 0, NULL, 1, 0, N'Tobahh', N'Tobahh', NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '6fc8b2c8-05ec-4db6-ae06-1d755ec3b948'");
        }
    }
}
