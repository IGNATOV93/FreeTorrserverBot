using System.IO;
using System.Diagnostics;
using FreeTorrserverBot.BotTelegram;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using FreeTorrBot.BotTelegram.BotSettings.Model;
using FreeTorrBot.BotTelegram.BotSettings;
using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using System.Text.Json;
using System;


namespace FreeTorrserverBot.Torrserver
{
    public abstract class Torrserver

    {
        static string  nameProcesTorrserver = "TorrServer-linux-amd64";
        static string filePathTorrMain = TelegramBot.settingsJson.FilePathTorrserver;
        static string filePathTorrserverDb = @$"{filePathTorrMain}accs.db";
        static string filePathTorr = @$"{filePathTorrMain}{nameProcesTorrserver}";
        static string filePathSettingsJson = @$"{filePathTorrMain}settings.json";

        public static async Task AutoChangeAccountTorrserver()
        {
            var settings = await SqlMethods.GetSettingsTorrserverBot();
            if (settings != null&&settings.IsActiveAutoChange==true)
            {
                var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                  {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});

                await ChangeAccountTorrserver("","",false,true);
                await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"Произведена автосмена пароля сервера \U00002705\r\n" +
                                                                                      $"\U0001F570   {DateTime.Now}", replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
                await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"{TakeAccountTorrserver()}", replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
            }
           
            return;
        }


        public static async Task ResetConfig()
        {
          await  WriteConfig(new BitTorrConfig() { IdChat=TelegramBot.AdminChat});
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
            return ;
        }
        public static async Task <BitTorrConfig> ReadConfig()
        {
            try
            {
                var jsonString = File.ReadAllText(filePathSettingsJson);
               // Console.WriteLine("Путь к settings.json: "+filePathSettingsJson);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Игнорирование регистра
                };

                var config = JsonSerializer.Deserialize<BitTorrConfigWrapper>(jsonString,options)?.BitTorr;
                if (config == null)
                {
                   throw  new Exception ("Ошибка не удалось загрузить конфигурацию из JSON");
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
        public static async Task ChangeAccountTorrserver(string login,string password,bool setLogin,bool setPassword)
        {
            var newParolRandom = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var settingsTorr = await SqlMethods.GetSettingsTorrserverBot();
            string newPassword = string.IsNullOrEmpty(password) ? settingsTorr.Password : password;
            string newLogin = string.IsNullOrEmpty(login) ? settingsTorr.Login : login;

            if (setLogin && string.IsNullOrEmpty(login))
            {
                newLogin = new string(Enumerable.Repeat(chars, 10)
                                .Select(s => s[newParolRandom.Next(s.Length)]).ToArray());
            }

            if (setPassword && string.IsNullOrEmpty(password))
            {
                newPassword = new string(Enumerable.Repeat(chars, 10)
                                .Select(s => s[newParolRandom.Next(s.Length)]).ToArray());
            }

            await SqlMethods.SetLoginPasswordSettingsTorrserverBot(newLogin, newPassword);
            string result = $"{{\"{newLogin}\":\"{newPassword}\"}}";

            using (StreamWriter writer = new StreamWriter(filePathTorrserverDb))
            {
                writer.WriteLine($"{result}");
            }

            await RebootingTorrserver();
        }
        public static async Task RebootingTorrserver()
        {
            // Завершаем процесс
            var killProcess = new ProcessStartInfo
            {
                FileName = "killall",
                Arguments = nameProcesTorrserver,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(killProcess))
            {
                await process.WaitForExitAsync(); // Ожидаем завершения процесса killall
            }

            // Запускаем процесс заново
            var startProcess = new ProcessStartInfo
            {
                FileName =$"{filePathTorrMain}{nameProcesTorrserver}", // Укажите полный путь к файлу, если он не в PATH
                UseShellExecute = true,
            };

            Process.Start(startProcess);
        }

        public static string TakeAccountTorrserver()
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePathTorrserverDb))
                {
                    string line;
                    string result = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        result += line;
                        Console.WriteLine(line);
                    }

                    return result.Replace("\"", "").Replace("{", "").Replace("}", "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "";
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
