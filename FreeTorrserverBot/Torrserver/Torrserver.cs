using System.IO;
using System.Diagnostics;
using FreeTorrserverBot.BotTelegram;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreeTorrserverBot.Torrserver
{
    public abstract class Torrserver
    {
        static string filePathTorrserverBd = @$"{TelegramBot.data["Profile0"]["FilePathTorrserverBd"]}";
        static string FilePathTor = @$"{TelegramBot.data["Profile0"]["FilePathTor"]}";
        public static async Task AutoChangeAccountTorrserver()
        {
            var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
               {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
            await ChangeAccountTorrserver();
            await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"Произведена автосмена пароля сервера \U00002705\r\n" +
                                                                                  $"\U0001F570   {DateTime.Now}",replyMarkup:inlineKeyboarDeleteMessageOnluOnebutton);
            await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"{TakeAccountTorrserver()}",replyMarkup:inlineKeyboarDeleteMessageOnluOnebutton);
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
        public static async Task RebootingTorrserver()
        {
            Process.Start("killall", "TorrServer-linux-amd64");
            Process.Start(@$"{FilePathTor}") ;
            return;
        }
    }
}
