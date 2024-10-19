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
        public string? TimeAutoChangePassword { get; set; }
        public string? FilePathTorrserverBd { get; set; }
        public string? FilePathTor { get; set; }

        public string? IsActiveAutoChange {get; set; }

        public string? LoginDefaultTorrserver { get; set; }
       
        // Переопределение метода ToString()
        public override string  ToString()
        {
            var timeServer = Torrserver.GetLocalServerTime();
            
            return

                   $"\u23F0 Автосмена пароля : {TimeAutoChangePassword ?? "не задан"}\r\n" +
                   $"Время сервера : {timeServer}\r\n" +
                  $"Статус автосмены: {(IsActiveAutoChange == "true" ? "✅" : "❌")}"
                   ;
        }
    }
}
