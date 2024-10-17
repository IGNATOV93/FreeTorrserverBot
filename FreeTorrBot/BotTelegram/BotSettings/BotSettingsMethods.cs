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
                case SettingsField.TimeAutoChangePassword:
                    settings.TimeAutoChangePassword = newValue;
                    break;
                case SettingsField.FilePathTorrserverBd:
                    settings.FilePathTorrserverBd = newValue;
                    break;
                case SettingsField.FilePathTor:
                    settings.FilePathTor = newValue;
                    break;
                case SettingsField.IsActiveAutoChange:
                    settings.IsActiveAutoChange = newValue;
                    break;
                case SettingsField.LoginDefaultTorrserver: settings.LoginDefaultTorrserver = newValue;
                    break;
            }

            SaveSettings(settings);
            TelegramBot.settingsJson = LoadSettings();
            Console.WriteLine($"{field} успешно обновлено.");
        }
        public static BotSettingsJson LoadSettings()
        {
           

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<BotSettingsJson>(json);
        }
        public static void SaveSettings(BotSettingsJson settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
