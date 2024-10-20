﻿using FreeTorrserverBot.BotTelegram;
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
            if(text == "💾 Бекапы")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat, "Настройки бекапов "

                    , replyMarkup: KeyboardManager.GetBackupMenu());
                return;
            }
            if (text =="\u2699 Настройки бота")
            {
                await DeleteMessage(idMessage);
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
                var settingsJson = BotSettingsMethods.LoadSettings();
                await SqlMethods.ListTablesAsync();
                await botClient.SendTextMessageAsync(AdminChat,
                    "Управление доступом к Torrserver.\r\n" + settingsJson.ToString()
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

            try
            {
                // Обрабатываем callback-сообщение
                if (callbackData == "deletemessages")
                {
                    await DeleteMessage(idMessage);
                    return;
                }
                if (callbackData == "сontrolTorrserver")
                {
                    
                    var settingsJson = BotSettingsMethods.LoadSettings();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Управление доступом к Torrserver.\r\n" + settingsJson.ToString()

                        , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "change_login")
                {

                    return;
                }

                if (callbackData == "change_password")
                {

                    await Torrserver.ChangeAccountTorrserver();
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
                    Console.WriteLine($"Ваш логин пароль {passw}");
                    return;
                }
                if (callbackData == "change_time_auto")
                {


                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , $"Установка автосмены пароля ."
                                                       , replyMarkup: KeyboardManager.GetSetTimeAutoChangePassword());
                }
                if (callbackData == "print_time_auto")
                {

                    var settings = LoadSettings();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , $"⏰ Время автосмены пароля {settings.TimeAutoChangePassword}"
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                }
                if (callbackData == "enable_auto_change")
                {

                    BotSettingsMethods.UpdateSettings(SettingsField.IsActiveAutoChange, "true");
                    var autoChangeTime = LoadSettings().TimeAutoChangePassword;
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , "Автосмена пароля включена \u2705 \r\n" +
                                                       autoChangeTime
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "disable_auto_change")
                {

                    BotSettingsMethods.UpdateSettings(SettingsField.IsActiveAutoChange, "false");
                    var autoChangeTime = LoadSettings().TimeAutoChangePassword;
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , "Автосмена пароля выключена \u274C\r\n" +
                                                       autoChangeTime
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "show_status")

                {

                    var settings = LoadSettings();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , settings.ToString()
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());

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
            };
            return commands.Contains(command);
        }
    }
}
