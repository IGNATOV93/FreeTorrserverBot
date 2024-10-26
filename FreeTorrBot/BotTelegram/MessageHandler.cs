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
using static System.Net.Mime.MediaTypeNames;
using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram;
using AdTorrBot.BotTelegram.Db.Model;

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
                Console.WriteLine($"Пришло text сообщение: { update?.Message?.Text}");
                // Проверка на режим ввода.
                var textInputFlags = await SqlMethods.GetTextInputFlag(AdminChat);
                if (textInputFlags.CheckAllBooleanFlags())
                {
                    await HandlerTextInputMessage(update.Message,textInputFlags);
                    return;
                }
                
                await HandlerTextButtonAndCommandMessage(update.Message);

                return;
            }
           if (update.Type == UpdateType.CallbackQuery)
            {
                Console.WriteLine($"Пришло Callback сообщение: {update.CallbackQuery.Data}");
                // Обработка callback-сообщения
                await HandlerCallbackQuery(update.CallbackQuery);
                return;
            }
           return;
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
        private static async Task HandlerTextInputMessage(Message message,TextInputFlag textInputFlag)
        {
            var text = message.Text;
            var idMessage = message.MessageId;
            // Обрабатываем обычное текстовое сообщение
            if (textInputFlag.FlagLogin)
            {
                Console.WriteLine("Обработка  TextInputFlagLogin");
                await DeleteMessage(idMessage);
                if (InputTextValidator.ValidateLogin(text))
                {
                    await Torrserver.ChangeAccountTorrserver(text);
                    await SqlMethods.SwitchTextInputFlagLogin(AdminChat, false);
                    Console.WriteLine("Смена логина выполнена.\r\n" +
                        "Вы вышли с режима ввода логина !");
                    await botClient.SendTextMessageAsync(AdminChat
                                                         , $"Ваш  новый логин \u27A1 {text}" +
                                                         " установлен \u2705"
                                                         , replyMarkup: KeyboardManager.GetDeleteThisMessage());
                }
                else
                {
                    Console.WriteLine("Смена логина не удалась.");
                    await botClient.SendTextMessageAsync(AdminChat
                                                          , "\u2757 Вы в режиме ввода логина .\r\n" +
                                                        "Напишите желаемый логин.\r\n" +
                                                        "\u2757 Login может содержать только английские буквы и цифры."
                                                        , replyMarkup: KeyboardManager.ExitTextLogin());
                }
                return;
            }
            return;
        }
        private static async Task HandlerTextButtonAndCommandMessage(Message message)
        {
            var text = message.Text;
            var idMessage = message.MessageId;
            // Обрабатываем обычное текстовое сообщение

            if (text == "/start")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat
                                                          , "Бот по управлению Torrserver приветствует тебя !"
                                                          , replyMarkup: KeyboardManager.GetMainKeyboard());
                return;
            }
            if(text == "💾 Бекапы")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat, "\u2699 Настройки бекапов \u2601"

                    , replyMarkup: KeyboardManager.GetMainBackups());
                return;
            }
            if (text =="\u2699 Настройки бота")
            {
                
                await botClient.SendTextMessageAsync(AdminChat, "\u2699 Настройки бота", replyMarkup: KeyboardManager.GetSettingsBot());
                return;

            }
            if(text== "⚙ Настройки Torrserver")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat, "\u2699 Настройки Torrserver");
                return;
            }
            if (text == "🔐 Доступ")
            {
                await DeleteMessage(idMessage);
                 var setTorr = await SqlMethods.GetSettingsTorrserverBot(AdminChat);
                await SqlMethods.ListTablesAsync();
                await botClient.SendTextMessageAsync(AdminChat,
                    "Управление доступом к Torrserver.\r\n" + setTorr.ToString()
                    , replyMarkup: KeyboardManager.GetControlTorrserver());
                return;
            }

            return;
        }

        private static async Task HandlerCallbackQuery(CallbackQuery callbackQuery)
        {
            var callbackData = callbackQuery.Data;
          //  Console.WriteLine(callbackData);
            var idMessage = callbackQuery.Message.MessageId;

            try
            {
                // Обрабатываем callback-сообщение
                if (callbackData == "deletemessages")
                {
                    await DeleteMessage(idMessage);
                    return;
                }
                if(callbackData== "exitTextLogin")
                {
                    await DeleteMessage(idMessage);
                    Console.WriteLine("Выход из ввода логина.");
                    await SqlMethods.SwitchTextInputFlagLogin(AdminChat,false);
                    await botClient.SendTextMessageAsync(AdminChat
                                                         , $"Вы вышли из режима ввода логина \u2705"
                                                         , replyMarkup: KeyboardManager.GetDeleteThisMessage());
                }
                if (callbackData == "сontrolTorrserver")
                {
                    Console.WriteLine("Управление доступом к Torrserver .");
                    var setTorr = await SqlMethods.GetSettingsTorrserverBot(AdminChat);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Управление доступом к Torrserver.\r\n" +setTorr.ToString()

                        , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "change_login")
                {
                    Console.WriteLine("В режите ввода логина.");
                    await SqlMethods.SwitchTextInputFlagLogin(AdminChat,true);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                        , "\u2757 Вы в режиме ввода логина .\r\n" +
                                                        "Напишите желаемый логин.\r\n" +
                                                        "\u2757 Login может содержать только английские буквы и цифры."
                                                        , replyMarkup: KeyboardManager.ExitTextLogin());
                    return;
                }

                if (callbackData == "change_password")
                {
                   
                    await Torrserver.ChangeAccountTorrserver("");
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                         , "Пароль успешно изменен !"
                                                         , replyMarkup: KeyboardManager.GetControlTorrserver());
                    Console.WriteLine("Пароль успешно изменен !");
                    return;
                }
                if (callbackData == "print_password" || callbackData == "print_login")
                {

                    var passw = Torrserver.TakeAccountTorrserver();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                             , $"{passw}"
                                                             , replyMarkup: KeyboardManager.GetControlTorrserver());
                    Console.WriteLine($"Запрос на просмотр логина:пароля удачен.");
                    return;
                }

                if (callbackData == "change_time_auto"||callbackData.Contains("setAutoPassMinutes"))
                {
                    
                        if (callbackData.Contains("setAutoPassMinutes"))
                        {
                            var setMinutesAutoChangePassTorr = ParsingMethods.ExtractTimeChangeValue(callbackData);
                            await SqlMethods.SetTimeAutoChangePasswordTorrserver(setMinutesAutoChangePassTorr);
                        }


                        var setTorr = await SqlMethods.GetSettingsTorrserverBot(AdminChat);
                        await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                           , $"Установка времени автосмены пароля .\r\n" +
                                                             $"Сейчас стоит {setTorr.TimeAutoChangePassword}"
                                                           , replyMarkup: KeyboardManager.GetSetTimeAutoChangePassword());
                    return;
                }
                if (callbackData == "print_time_auto")
                {

                    var setTorr =await SqlMethods.GetSettingsTorrserverBot(AdminChat);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , $"⏰ Время автосмены пароля {setTorr.TimeAutoChangePassword}"
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "enable_auto_change")
                {
                    var setTorr = await SqlMethods.GetSettingsTorrserverBot(AdminChat);
                    await SqlMethods.SwitchAutoChangePassTorrserver(true,AdminChat);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , "Автосмена пароля включена \u2705 \r\n" +
                                                      setTorr.TimeAutoChangePassword
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "disable_auto_change")
                {

                    var setTorr = await SqlMethods.GetSettingsTorrserverBot(AdminChat);
                    await SqlMethods.SwitchAutoChangePassTorrserver(false, AdminChat);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , "Автосмена пароля выключена \u2705 \r\n" +
                                                      setTorr.TimeAutoChangePassword
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "show_status")

                {

                    var settings = await SqlMethods.GetSettingsTorrserverBot(AdminChat);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , settings.ToString()
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());    
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
            }

            
            return;
        }

        public static bool IsTextCommandBot(string command)
        {
            HashSet<string> commands = new HashSet<string>()
            {
             "/start"
             ,"🔐 Доступ"
             ,"💾 Бекапы"
             ,"\u2699 Настройки бота"
             ,"⚙ Настройки Torrserver"
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
            ,"change_login"
            ,"print_login"
            ,"сontrolTorrserver"
            ,"setAutoPassMinutes"
            ,"exitTextLogin"
            };
            // Проверяем, если это одна из стандартных команд
            if (commands.Contains(command))
            {
                return true;
            }

            // Если команда содержит "setAutoPassMinutes", проверим формат
            if (command.Contains("setAutoPassMinutes"))
            {
                // Получаем часть команды перед "setAutoPassMinutes"
                string valuePart = command.Split("setAutoPassMinutes")[0].Trim();

                // Проверяем, является ли часть числовым значением (может быть с + или -)
                if (int.TryParse(valuePart, out _))
                {
                    return true;
                }
            }

            // Если команда не найдена
            return false;
        }
    }
}
