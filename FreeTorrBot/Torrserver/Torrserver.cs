using System.IO;
using System.Diagnostics;
using FreeTorrserverBot.BotTelegram;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using FreeTorrBot.BotTelegram.BotSettings.Model;
using FreeTorrBot.BotTelegram.BotSettings;

namespace FreeTorrserverBot.Torrserver
{
    public abstract class Torrserver
    {
        static string filePathTorrserverBd = @$"{TelegramBot.settingsJson.FilePathTorrserverBd}";
        static string FilePathTor = @$"{TelegramBot.settingsJson.FilePathTor}";
        public static async Task AutoChangeAccountTorrserver()
        {
            var settings = BotSettingsMethods.LoadSettings();
            if (settings != null&&settings.IsActiveAutoChange=="true")
            {
                var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                  {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
                await ChangeAccountTorrserver();
                await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"Произведена автосмена пароля сервера \U00002705\r\n" +
                                                                                      $"\U0001F570   {DateTime.Now}", replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
                await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"{TakeAccountTorrserver()}", replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
            }
           
            return;
        }
        public static async Task ChangeAccountTorrserver()
        {
            var newParolRandom = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var newParol = new string(Enumerable.Repeat(chars, 8)
                         .Select(s => s[newParolRandom.Next(s.Length)]).ToArray());
            string result = $"{{\"freeServer\":\"{newParol}\"}}";
            using (StreamWriter writer = new StreamWriter(filePathTorrserverBd))
            {
                writer.WriteLine($"{result}");
            }
            await RebootingTorrserver();
        }
        public static string TakeAccountTorrserver()
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePathTorrserverBd))
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
        public static async Task RebootingTorrserver()
        {
            var nameProcesTorrserver = BotTelegram.TelegramBot.settingsJson.FilePathTor.Substring(BotTelegram.TelegramBot.settingsJson.FilePathTor.LastIndexOf('/') + 1);
            Process.Start("killall",nameProcesTorrserver);
            Process.Start(@$"{FilePathTor}") ;
            return;
        }
    }
}
