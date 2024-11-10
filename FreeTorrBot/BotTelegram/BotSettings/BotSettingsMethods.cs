using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeTorrBot.BotTelegram.BotSettings.Model;
using FreeTorrserverBot.BotTelegram;

namespace FreeTorrBot.BotTelegram.BotSettings
{
    public  abstract class BotSettingsMethods
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public  enum SettingsField
        {
            YourBotTelegramToken,
            AdminChatId,
            TimeAutoChangePassword,
            FilePathTorrserverBd,
            FilePathTor,
            IsActiveAutoChange,
            LoginDefaultTorrserver
        }
        public static void UpdateSettings(SettingsField field, string newValue)
        {
            var settings = LoadSettings();

            if (settings == null) return;

            switch (field)
            {
                case SettingsField.YourBotTelegramToken:
                    settings.YourBotTelegramToken = newValue;
                    break;
                case SettingsField.AdminChatId:
                    settings.AdminChatId = newValue;
                    break;
                case SettingsField.FilePathTorrserverBd:
                    settings.FilePathTorrserver = newValue;
                    break;
               
            }

            SaveSettings(settings);
            TelegramBot.settingsJson = LoadSettings();
            Console.WriteLine($"{field} успешно обновлено.");
        }
        public static BotSettingsJson LoadSettings()
        {
            var json = File.ReadAllText(path);
            var settings = JsonConvert.DeserializeObject<BotSettingsJson>(json);

            // Проверка, что настройки загружены
            if (settings == null)
            {
                throw new InvalidOperationException("Не удалось загрузить настройки: настройки равны null.");
            }

            // Валидация загруженных настроек
            settings.Validate();

            // Возвращаем валидные настройки
            return settings;
        }
        public static void SaveSettings(BotSettingsJson settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
