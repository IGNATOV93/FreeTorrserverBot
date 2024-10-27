using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTorrBot.Migrations
{
    /// <inheritdoc />
    public partial class _2710 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FlagPassword",
                table: "TextInputFlag",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlagPassword",
                table: "TextInputFlag");
        }
    }
}
