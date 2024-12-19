using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using AdTorrBot.BotTelegram.Db.Model;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
namespace AdTorrBot.BotTelegram.Db
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<SettingsBot> SettingsBot {  get; set; }
        public virtual DbSet<SettingsTorrserverBot> SettingsTorrserverBot { get; set; }
        public virtual DbSet<User>User { get; set; }
        public virtual DbSet<TextInputFlag> TextInputFlag { get; set; }
        public virtual DbSet<BitTorrConfig> BitTorrConfig { get; set; }
        public virtual DbSet<ServerArgsConfig> ServerArgsConfig { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Подключение к SQLite базе данных
            optionsBuilder.UseSqlite("Data Source=app.db");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Устанавливаем уникальность для пары полей IdChat и NameProfileBot
            modelBuilder.Entity<BitTorrConfig>()
                .HasIndex(b => new { b.IdChat, b.NameProfileBot })
                .IsUnique();
        }

    }
}
