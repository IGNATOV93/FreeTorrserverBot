using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTorrBot.Migrations
{
    /// <inheritdoc />
    public partial class _07112024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BitTorrConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdChat = table.Column<string>(type: "TEXT", nullable: false),
                    NameProfileBot = table.Column<string>(type: "TEXT", nullable: false),
                    CacheSize = table.Column<string>(type: "TEXT", nullable: false),
                    ConnectionsLimit = table.Column<string>(type: "TEXT", nullable: false),
                    DisableDHT = table.Column<string>(type: "TEXT", nullable: false),
                    DisablePEX = table.Column<string>(type: "TEXT", nullable: false),
                    DisableTCP = table.Column<string>(type: "TEXT", nullable: false),
                    DisableUPNP = table.Column<string>(type: "TEXT", nullable: false),
                    DisableUTP = table.Column<string>(type: "TEXT", nullable: false),
                    DisableUpload = table.Column<string>(type: "TEXT", nullable: false),
                    DownloadRateLimit = table.Column<string>(type: "TEXT", nullable: false),
                    EnableDLNA = table.Column<string>(type: "TEXT", nullable: false),
                    EnableDebug = table.Column<string>(type: "TEXT", nullable: false),
                    EnableIPv6 = table.Column<string>(type: "TEXT", nullable: false),
                    EnableRutorSearch = table.Column<string>(type: "TEXT", nullable: false),
                    ForceEncrypt = table.Column<string>(type: "TEXT", nullable: false),
                    FriendlyName = table.Column<string>(type: "TEXT", nullable: false),
                    PeersListenPort = table.Column<string>(type: "TEXT", nullable: false),
                    PreloadCache = table.Column<string>(type: "TEXT", nullable: false),
                    ReaderReadAHead = table.Column<string>(type: "TEXT", nullable: false),
                    RemoveCacheOnDrop = table.Column<string>(type: "TEXT", nullable: false),
                    ResponsiveMode = table.Column<string>(type: "TEXT", nullable: false),
                    RetrackersMode = table.Column<string>(type: "TEXT", nullable: false),
                    SslCert = table.Column<string>(type: "TEXT", nullable: false),
                    SslKey = table.Column<string>(type: "TEXT", nullable: false),
                    SslPort = table.Column<string>(type: "TEXT", nullable: false),
                    TorrentDisconnectTimeout = table.Column<string>(type: "TEXT", nullable: false),
                    TorrentsSavePath = table.Column<string>(type: "TEXT", nullable: false),
                    UploadRateLimit = table.Column<string>(type: "TEXT", nullable: false),
                    UseDisk = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitTorrConfig", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitTorrConfig");
        }
    }
}
