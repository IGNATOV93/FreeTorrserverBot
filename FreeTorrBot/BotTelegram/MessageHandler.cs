using FreeTorrserverBot.BotTelegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

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
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return;
        }
        private static async Task HandleTextMessage(Message message)
        {
            var text = message.Text;
            var idMessage = message.MessageId;
            // Обрабатываем обычное текстовое сообщение
            if (text == "/start")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать!");
            }

            return;
        }

        private static async Task HandleCallbackQuery(CallbackQuery callbackQuery)
        {
            var callbackData = callbackQuery.Data;
            var idMessage = callbackQuery.Message.MessageId;
            // Обрабатываем callback-сообщение
            if (callbackData == "deletemessages")
            {
                await DeleteMessage(idMessage);
            }
            if (callbackData == "admin_menu")
            {
                await botClient.SendTextMessageAsync(AdminChat,"Вы открыли меню управления.",
                    replyMarkup:KeyboardManager.GetAdminKeyboard());
            }
          
           
            // Подтверждаем обработку callback-запроса
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
