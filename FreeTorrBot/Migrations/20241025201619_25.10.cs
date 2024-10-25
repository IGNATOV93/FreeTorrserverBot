using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTorrBot.Migrations
{
    /// <inheritdoc />
    public partial class _2510 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SettingsBot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdChat = table.Column<string>(type: "TEXT", nullable: false),
                    TimeZoneOffset = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsBot", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "TextInputFlag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdChat = table.Column<string>(type: "TEXT", nullable: true),
                    FlagLogin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextInputFlag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdChat = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingsBot");

            migrationBuilder.DropTable(
                name: "SettingsTorrserverBot");

            migrationBuilder.DropTable(
                name: "TextInputFlag");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
