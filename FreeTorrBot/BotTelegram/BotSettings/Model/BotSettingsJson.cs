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
        public string? FilePathTorrserver { get; set; }
        // Метод для проверки корректности настроек
        public bool Validate(out List<string> missingFields)
        {
            missingFields = new List<string>();

            if (string.IsNullOrWhiteSpace(YourBotTelegramToken))
                missingFields.Add("YourBotTelegramToken");

            if (string.IsNullOrWhiteSpace(AdminChatId))
                missingFields.Add("AdminChatId");

            if (string.IsNullOrWhiteSpace(FilePathTorrserver))
                missingFields.Add("FilePathTorrserver");

            return missingFields.Count == 0;
        }

    }

}
