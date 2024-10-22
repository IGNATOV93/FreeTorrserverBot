using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTorrBot.Migrations
{
    /// <inheritdoc />
    public partial class _2310 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingsTorrserver");

            migrationBuilder.CreateTable(
                name: "SettingsTorrserverBot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idChat = table.Column<string>(type: "TEXT", nullable: true),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    IsActiveAutoChange = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeAutoChangePassword = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsTorrserverBot", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingsTorrserverBot");

            migrationBuilder.CreateTable(
                name: "SettingsTorrserver",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsTorrserver", x => x.Id);
                });
        }
    }
}
