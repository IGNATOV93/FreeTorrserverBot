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
        public string? TimeAutoChangePassword { get; set; }
        public string? FilePathTorrserverBd { get; set; }
        public string? FilePathTor { get; set; }

        public string? IsActiveAutoChange {get; set; }

        // Переопределение метода ToString()
        public override string ToString()
        {
            return
                 
                   $"\u23F0 Автосмена пароля : {TimeAutoChangePassword ?? "не задан"}\r\n\r\n" +
                  $"Статус автосмены: {(IsActiveAutoChange == "true" ? "✅" : "❌")}"
                   ;
        }
    }
}
