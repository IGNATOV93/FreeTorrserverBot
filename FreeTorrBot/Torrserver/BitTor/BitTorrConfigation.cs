using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using AdTorrBot.BotTelegram.Db;
using FreeTorrserverBot.BotTelegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FreeTorrserverBot.Torrserver.Torrserver;

namespace FreeTorrserverBot.Torrserver.BitTor
{
    public class BitTorrConfigation
    {
        static string nameProcesTorrserver = "TorrServer-linux-amd64";
        static string filePathTorrMain = TelegramBot.settingsJson.FilePathTorrserver;
        static string filePathTorrserverDb = @$"{filePathTorrMain}accs.db";
        static string filePathTorr = @$"{filePathTorrMain}{nameProcesTorrserver}";
        static string filePathSettingsJson = @$"{filePathTorrMain}settings.json";
        public static async Task ResetConfig()
        {
            await WriteConfig(new BitTorrConfig() { IdChat = TelegramBot.AdminChat });
            return;
        }
        public static async Task WriteConfig(BitTorrConfig config)
        {
            try
            {
                // Обернуть объект config в объект-обертку для соблюдения JSON структуры
                var wrapper = new BitTorrConfigWrapper(config);
                wrapper.BitTorr.Id = 0;


                // Сериализация объекта в JSON
                var jsonString = JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = true });

                // Запись JSON в файл
                File.WriteAllText(filePathSettingsJson, jsonString);
                await SqlMethods.SetSettingsTorrProfile(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи JSON: {ex.Message}");
                return;
            }
            return;
        }
        public static async Task<BitTorrConfig> ReadConfig()
        {
            try
            {
                var jsonString = File.ReadAllText(filePathSettingsJson);
                // Console.WriteLine("Путь к settings.json: "+filePathSettingsJson);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Игнорирование регистра
                };

                var config = JsonSerializer.Deserialize<BitTorrConfigWrapper>(jsonString, options)?.BitTorr;
                if (config == null)
                {
                    throw new Exception("Ошибка не удалось загрузить конфигурацию из JSON");
                }

                await SqlMethods.SetSettingsTorrProfile(config);

                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public class BitTorrConfigWrapper
        {
            public BitTorrConfig BitTorr { get; set; }

            // Убедитесь, что конструктор без параметров
            public BitTorrConfigWrapper() { }

            // Конструктор с параметром для удобства
            public BitTorrConfigWrapper(BitTorrConfig config)
            {
                BitTorr = config;  // Просто сохраняем ссылку на объект
            }
        }
    }
}
