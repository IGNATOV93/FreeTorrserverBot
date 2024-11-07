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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Подключение к SQLite базе данных
            optionsBuilder.UseSqlite("Data Source=app.db");
            
        }
  
    }
}
