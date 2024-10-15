using FreeTorrserverBot.BotTelegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using FreeTorrserverBot.Torrserver;
using FreeTorrBot.BotTelegram.BotSettings;
using static FreeTorrBot.BotTelegram.BotSettings.BotSettingsMethods;

namespace FreeTorrBot.BotTelegram
{
    public abstract class MessageHandler
    {
        private static TelegramBotClient botClient = TelegramBot.client;
        private readonly static string AdminChat = TelegramBot.AdminChat;
        public static async Task HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                // Обработка обычного текстового сообщения
                await HandleTextMessage(update.Message);
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                // Обработка callback-сообщения
                await HandleCallbackQuery(update.CallbackQuery);
            }
        }

        public static async Task DeleteMessage(int messageId)
        {
            try
            {
                await botClient.DeleteMessageAsync(AdminChat, messageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return;
        }
        private static async Task HandleTextMessage(Message message)
        {
            var text = message.Text;
            var idMessage = message.MessageId;
           // Console.WriteLine(text);
            // Обрабатываем обычное текстовое сообщение
            if (text == "/start")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat
                                                          , "Бот по управлению Torrserver приветствует тебя !"
                                                          , replyMarkup: KeyboardManager.GetMainKeyboard());
                return;
            }
            if (text == "🛠 Управление")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat, "Вы открыли меню управления torrserver."
                    , replyMarkup: KeyboardManager.GetControlTorrserver());
                return;
            }
            return;
        }

        private static async Task HandleCallbackQuery(CallbackQuery callbackQuery)
        {
            var callbackData = callbackQuery.Data;
          //  Console.WriteLine(callbackData);
            var idMessage = callbackQuery.Message.MessageId;
            // Обрабатываем callback-сообщение
            if (callbackData == "deletemessages")
            {
                await DeleteMessage(idMessage);
                return;
            }
            if(callbackData== "change_password")
            {
                await DeleteMessage(idMessage);
                await Torrserver.ChangeAccountTorrserver();
                await botClient.SendTextMessageAsync(AdminChat
                                                     , "Пароль успешно изменен !"
                                                     , replyMarkup: KeyboardManager.GetDeleteThisMessage());
                Console.WriteLine("Пароль успешно изменен !");
                return;
            }    
            if(callbackData== "print_password")
            {
                await DeleteMessage(idMessage);
                var passw = Torrserver.TakeAccountTorrserver();
                await botClient.SendTextMessageAsync(AdminChat
                                                         , $"{passw}"
                                                         , replyMarkup: KeyboardManager.GetDeleteThisMessage());
                Console.WriteLine($"Ваш логин пароль {passw}");
                return ;
            }
            if(callbackData== "change_time_auto")
            {
                await DeleteMessage(idMessage);
                
                await botClient.SendTextMessageAsync(AdminChat
                                                   , $"Данная функция временно не работает,\r\n"+
                                                      "Обновите вручную через settings.json"
                                                   , replyMarkup: KeyboardManager.GetDeleteThisMessage());
            }
            if(callbackData== "print_time_auto")
            {
                await DeleteMessage(idMessage);
                var settings = LoadSettings();
                await botClient.SendTextMessageAsync(AdminChat
                                                   , $"⏰ Время автосмены пароля {settings.TimeAutoChangePassword}"
                                                   , replyMarkup: KeyboardManager.GetDeleteThisMessage());
            }
            if(callbackData== "enable_auto_change")
            {
                await DeleteMessage(idMessage);
                BotSettingsMethods.UpdateSettings(SettingsField.IsActiveAutoChange, "true");
              var autoChangeTime = LoadSettings().TimeAutoChangePassword;
                await botClient.SendTextMessageAsync(AdminChat
                                                   , "Автосмена пароля включена \u2705 \r\n" +
                                                   autoChangeTime
                                                   , replyMarkup: KeyboardManager.GetDeleteThisMessage());
                return ;
            }
            if(callbackData== "disable_auto_change")
            {
                await DeleteMessage(idMessage);
                BotSettingsMethods.UpdateSettings(SettingsField.IsActiveAutoChange, "false");
                var autoChangeTime = LoadSettings().TimeAutoChangePassword;
                await botClient.SendTextMessageAsync(AdminChat
                                                   , "Автосмена пароля выключена \u274C\r\n" +
                                                   autoChangeTime
                                                   , replyMarkup: KeyboardManager.GetDeleteThisMessage());
                return;
            }
            if(callbackData== "show_status")
               
            {
                await DeleteMessage(idMessage);
                var settings =LoadSettings();
                await botClient.SendTextMessageAsync(AdminChat
                                                   , settings.ToString()
                                                   , replyMarkup: KeyboardManager.GetDeleteThisMessage());

            }
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
            return;
        }

        public static bool IsTextCommandBot(string command)
        {
            HashSet<string> commands = new HashSet<string>()
            {
             "/start"
             ,"🛠 Управление"
            };
            return commands.Contains(command);

        }
        public static bool IsCallbackQueryCommandBot(string command)
        {
            HashSet<string> commands = new HashSet<string>()
            {
            "deletemessages"
            ,"change_password"
            ,"print_password"
            ,"change_time_auto"
            ,"print_time_auto"
            , "enable_auto_change"
            ,"disable_auto_change"
            ,"show_status"
            };
            return commands.Contains(command);
        }
    }
}
