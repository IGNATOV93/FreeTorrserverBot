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
        // Метод для проверки корректности настроек
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(YourBotTelegramToken))
                throw new InvalidOperationException("Необходимо указать токен бота Telegram.");

            if (string.IsNullOrWhiteSpace(AdminChatId))
                throw new InvalidOperationException("Необходимо указать ID чата администратора.");

            if (string.IsNullOrWhiteSpace(FilePathTorrserverBd))
                throw new InvalidOperationException("Необходимо указать путь к accs.db .");

            if (string.IsNullOrWhiteSpace(FilePathTor))
                throw new InvalidOperationException("Необходимо указать путь к исполняемому файлу Torrserver.");
        }
    }
    
}
