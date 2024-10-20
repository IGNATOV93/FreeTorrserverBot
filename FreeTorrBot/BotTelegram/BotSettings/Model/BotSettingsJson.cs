using AdTorrBot.BotTelegram.Db;
using FreeTorrserverBot.Torrserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTorrBot.BotTelegram.BotSettings.Model
{
    public class BotSettingsJson
    {
        public string? YourBotTelegramToken { get; set; }
        public string? AdminChatId { get; set; }
        public string? FilePathTorrserverBd { get; set; }
        public string? FilePathTor { get; set; }

    }
    
}
