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
using AdTorrBot.ServerManagement;
using FreeTorrBot.BotTelegram;
using FreeTorrserverBot.Torrserver.BitTor;


namespace AdTorrBot.BotTelegram.Handler
{
    public abstract class MessageHandler
    {
        protected static TelegramBotClient botClient = TelegramBot.client;
        protected readonly static string AdminChat = TelegramBot.AdminChat;
        public static async Task HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                Console.WriteLine($"Пришло text сообщение: {update?.Message?.Text}");
                // Проверка на режим ввода.
                var textInputFlags = await SqlMethods.GetTextInputFlag();
                if (textInputFlags.CheckAllBooleanFlags())
                {
                    await HandlerCallbackQueryTorrSett.HandlerTextInputMessage(update.Message, textInputFlags);
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
            if (text == "💾 Бекапы")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat, "\u2699 Настройки бекапов \u2601"

                    , replyMarkup: KeyboardManager.GetMainBackups());
                return;
            }

            if (text == "⚙ Настройки")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat, "\u2699 Настройки", replyMarkup: KeyboardManager.GetSettingsMain());
                return;
            }
            if (text == "🔐 Доступ")
            {
                await DeleteMessage(idMessage);
                var setTorr = await SqlMethods.GetSettingsTorrserverBot();
                await SqlMethods.ListTablesAsync();
                await botClient.SendTextMessageAsync(AdminChat,
                    "Управление доступом к Torrserver.\r\n" + setTorr.ToString()
                    , replyMarkup: KeyboardManager.GetControlTorrserver());
                return;
            }
            if (text == "🔄 Перезагрузки")
            {
                await DeleteMessage(idMessage);
                await botClient.SendTextMessageAsync(AdminChat, "🔄 Перезагрузки", replyMarkup: KeyboardManager.GetRestartingMain());
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
                if (callbackData == "restart_torrserver")
                {
                    await Torrserver.RebootingTorrserver();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Torrserver перезагружен \u2705", replyMarkup: KeyboardManager.buttonHideButtots);
                    return;
                }
                if (callbackData == "restart_server")
                {

                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Server будет перезагружен \u2705", replyMarkup: KeyboardManager.buttonHideButtots);
                    ServerControl.RebootServer();
                    //////Сделать вызов метода по перезагрузке сервера.
                    return;
                }
                if (callbackData == "back_settings_main")
                {

                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "\u2699 Настройки", replyMarkup: KeyboardManager.GetSettingsMain());
                    return;
                }
               

                
                if (callbackData.Contains("torrSetOne"))
                {
                    
                    await HandlerCallbackQueryTorrSett.CheckSettingAndExecute(callbackQuery, callbackData);

                    return;
                }
                if (callbackData == "setTorrSetConfig")
                {
                    await Torrserver.RebootingTorrserver();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Настройки Torrserver обновленны! \u2705\r\n" +
                        "Torrserver перезагружен .", replyMarkup: KeyboardManager.GetShoWTorrConfig());
                    return;
                }
                if (callbackData == "resetTorrSetConfig")
                {
                    //ВЫЗОВ МЕТОДА ДЛЯ СБРОСА НАСТРОЕК TORRSERVER .
                    await BitTorrConfigation.ResetConfig();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Настройки Torrserver сброшены по умолчанию ! \u2705", replyMarkup: KeyboardManager.GetShoWTorrConfig());
                    return;
                }
                if (callbackData == "showTorrsetInfo")
                {
                    var config = await SqlMethods.GetSettingsTorrProfile(AdminChat);
                    var resultInfoTorrSettings = config.ToString();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, resultInfoTorrSettings, replyMarkup: KeyboardManager.GetShoWTorrConfig());
                    return;
                }
                if (callbackData.Contains("torrSettings"))
                {
                    var startIndexKeySettings = Convert.ToInt32(callbackData.Split("torrSettings")[0]);
                    await SqlMethods.SwitchOffInputFlag();
                    var config = await SqlMethods.GetSettingsTorrProfile(AdminChat);
                    Console.WriteLine("Настройки Torrserver");
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "⚙️ Настройки Torrserver ."
                        , replyMarkup:await KeyboardManager.GetBitTorrConfigMain(AdminChat, config, startIndexKeySettings)
                        );
                    return;
                }
                if (callbackData.Contains("torrArgsSettings"))
                {
                    // torr_config
                    var startIndexKeySettings = Convert.ToInt32(callbackData.Split("torrArgsSettings")[0]);
                    await SqlMethods.SwitchOffInputFlag();
                    var config = await SqlMethods.GetArgsConfigTorrProfile(AdminChat);
                    Console.WriteLine("Настройки Torrserver");
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "🛠️ Конфиг Torrserver ."
                        , replyMarkup: KeyboardManager.GetTorrConfigMain(AdminChat, config, startIndexKeySettings)
                        );
                    return;
                }
                if (callbackData.Contains("set_server_bbr"))
                {
                    bool? enable = callbackData.StartsWith("1") ? true
                                 : callbackData.StartsWith("0") ? false
                                 : null;

                    if (enable.HasValue)
                    {
                        await ServerControl.SetBbrState(enable.Value);
                    }

                    var isBBRActive = ServerInfo.CheckBBRConfig();
                    string bbrStatus = isBBRActive ? "\u2705" : "\u274C";

                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Состояние BBR на сервере: " + bbrStatus,
                        replyMarkup: KeyboardManager.GetSetServerBbrMain(isBBRActive));
                    return;
                }

                if (callbackData == "set_server")
                {
                    Console.WriteLine("Настройки сервера");
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "💻 Настройки сервера"
                        , replyMarkup: KeyboardManager.GetSetServerMain());
                    return;
                }
                if (callbackData == "set_bot")
                {
                    Console.WriteLine("Настройки бота");
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "🤖 Настройки бота"
                        , replyMarkup: KeyboardManager.GetSettingsBot()
                        );
                    return;
                }

                if (callbackData.Contains("time_zone"))
                {
                    var timezoneChangeIndicator = callbackData.Split("time_zone")[0];
                    Console.WriteLine($"Пришел timezoneChangeIndicator: {timezoneChangeIndicator}");
                    // Проверяем, содержит ли значение "+" или "-"
                    if (timezoneChangeIndicator == "+" || timezoneChangeIndicator == "-")
                    {
                        // Вызываем метод для смены часового пояса
                        await SqlMethods.SwitchTimeZone(timezoneChangeIndicator);
                    }
                    var settingBot = await SqlMethods.GetSettingBot();
                    var timeLocalServer = ServerInfo.GetLocalServerTime();
                    var localTimeZone = ServerInfo.GetLocalServerTimeTimeZone();
                    string localTimeZoneString = $"UTC{(localTimeZone >= 0 ? "+" : "")}{localTimeZone}";
                    await botClient.EditMessageTextAsync(AdminChat, idMessage,

                        $"\uD83D\uDD52 Время сервера : {timeLocalServer}  🌍 {localTimeZoneString}\r\n" +
                        $"🌍 Ваш часовой пояс: {settingBot.TimeZoneOffset} UTC",
                        replyMarkup: KeyboardManager.GetMainTimeZone());
                    return;

                }

                if (callbackData.Contains("exit"))
                {
                    var property = callbackData.Split("exit")[1];
                    await DeleteMessage(idMessage);
                    Console.WriteLine($"Выход из ввода {property}");
                    await SqlMethods.SwitchTorSettingsInputFlag(property, false);
                    var result = ParsingMethods.GetExitMessage(property);
                    await botClient.SendTextMessageAsync(AdminChat
                                                         , result
                                                         , replyMarkup: KeyboardManager.GetDeleteThisMessage());
                    return;
                }
                if (callbackData == "manage_login_password")
                {
                    Console.WriteLine("Управление логином и паролем Torrserver");
                    await SqlMethods.SwitchOffInputFlag();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Управление логином и паролем Torrserver ."

                     , replyMarkup: KeyboardManager.GetNewLoginPasswordMain());
                    return;
                }
                if (callbackData == "сontrolTorrserver")
                {
                    Console.WriteLine("Управление доступом к Torrserver .");
                    var setTorr = await SqlMethods.GetSettingsTorrserverBot();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage, "Управление доступом к Torrserver.\r\n" + setTorr.ToString()

                        , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }

                if (callbackData == "set_login_manually")
                {
                    Console.WriteLine("В режите ввода логина.");
                    await SqlMethods.SwitchTorSettingsInputFlag("FlagLogin",true);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                        , "\u2757 Вы в режиме ввода логина .\r\n" +
                                                        "Напишите желаемый логин.\r\n" +
                                                        "\u2757 Login может содержать только английские буквы и цифры.\r\n" +
                                                         "Ограничение на символы:10"
                                                        , replyMarkup: KeyboardManager.CreateExitTorrSettInputButton("Login", 0 ));
                    return;
                }
                if (callbackData == "set_password_manually")
                {
                    Console.WriteLine("В режите ввода пароля.");
                    await SqlMethods.SwitchTorSettingsInputFlag("FlagPassword",true);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                        , "\u2757 Вы в режиме ввода пароля .\r\n" +
                                                        "Напишите желаемый пароль.\r\n" +
                                                        "\u2757 Password может содержать только английские буквы и цифры.\r\n" +
                                                         "Ограничение на символы:10"
                                                        , replyMarkup: KeyboardManager.CreateExitTorrSettInputButton("Password",0));
                    return;

                }
                if (callbackData == "generate_new_login")
                {
                    await Torrserver.ChangeAccountTorrserver("", "", true, false);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                         , "Логин успешно изменен !"
                                                         , replyMarkup: KeyboardManager.GetNewLoginPasswordMain());
                    Console.WriteLine("Логин успешно изменен !");
                    return;
                }
                if (callbackData == "generate_new_password")
                {

                    await Torrserver.ChangeAccountTorrserver("", "", false, true);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                         , "Пароль успешно изменен !"
                                                         , replyMarkup: KeyboardManager.GetNewLoginPasswordMain());
                    Console.WriteLine("Пароль успешно изменен !");
                    return;
                }
                if (callbackData == "show_login_password")
                {

                    var passw = Torrserver.TakeAccountTorrserver();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                             , $"{passw}"
                                                             , replyMarkup: KeyboardManager.GetNewLoginPasswordMain());
                    Console.WriteLine($"Запрос на просмотр логина:пароля удачен.");
                    return;
                }

                if (callbackData == "change_time_auto" || callbackData.Contains("setAutoPassMinutes"))
                {

                    if (callbackData.Contains("setAutoPassMinutes"))
                    {
                        var setMinutesAutoChangePassTorr = ParsingMethods.ExtractTimeChangeValue(callbackData);
                        await SqlMethods.SetTimeAutoChangePasswordTorrserver(setMinutesAutoChangePassTorr);
                    }


                    var setTorr = await SqlMethods.GetSettingsTorrserverBot();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , $"Установка времени автосмены пароля .\r\n" +
                                                         $"Сейчас стоит {setTorr.TimeAutoChangePassword}"
                                                       , replyMarkup: KeyboardManager.GetSetTimeAutoChangePassword());
                    return;
                }
                if (callbackData == "print_time_auto")
                {

                    var setTorr = await SqlMethods.GetSettingsTorrserverBot();
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , $"⏰ Время автосмены пароля {setTorr.TimeAutoChangePassword}"
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "enable_auto_change")
                {
                    var setTorr = await SqlMethods.GetSettingsTorrserverBot();
                    await SqlMethods.SwitchAutoChangePassTorrserver(true);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , "Автосмена пароля включена \u2705 \r\n" +
                                                      setTorr.TimeAutoChangePassword
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "disable_auto_change")
                {

                    var setTorr = await SqlMethods.GetSettingsTorrserverBot();
                    await SqlMethods.SwitchAutoChangePassTorrserver(false);
                    await botClient.EditMessageTextAsync(AdminChat, idMessage
                                                       , "Автосмена пароля выключена \u2705 \r\n" +
                                                      setTorr.TimeAutoChangePassword
                                                       , replyMarkup: KeyboardManager.GetControlTorrserver());
                    return;
                }
                if (callbackData == "show_status")

                {

                    var settings = await SqlMethods.GetSettingsTorrserverBot();
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
           //  ,"\u2699 Настройки бота"
             ,"⚙ Настройки"
             ,"🔄 Перезагрузки"
            };
            return commands.Contains(command);

        }
        public static bool IsCallbackQueryCommandBot(string command)
        {
            HashSet<string> commands = new HashSet<string>()
            {
            "deletemessages"
            //,"change_password"
            ,"manage_login_password"
            ,"generate_new_password"
            ,"set_password_manually"
            ,"generate_new_login"
            ,"set_login_manually"
            ,"show_login_password"
            //,"print_password"
            ,"change_time_auto"
            ,"print_time_auto"
            , "enable_auto_change"
            ,"disable_auto_change"
            ,"show_status"
            //,"change_login"
            //,"print_login"
            ,"сontrolTorrserver"
            ,"setAutoPassMinutes"
            ,"-time_zone"
            ,"+time_zone"
            ,"time_zone"
            //Настройки
          //  ,"torr_settings"
            ,"torrArgsSettings"
            ,"set_server"
            ,"set_bot"

            ,"back_settings_main"
            ,"set_server_bbr"

            ,"restart_torrserver"
            ,"restart_server"
            
            ,"showTorrsetInfo"
            ,"resetTorrSetConfig"
            ,"setTorrSetConfig"
            };
            // Проверяем, если это одна из стандартных команд
            if (commands.Contains(command))
            {
                return true;
            }
            if (commands.Contains("exit"))
            {
                return true;
            }
            if (commands.Contains("TorrSett"))
            {
                return true ;
            }
            if (commands.Contains("torrArgsSettings"))
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
            if (command.Contains("torrSettings"))
            {
                return true;
            }
            if (command.Contains("torrSetOne"))
            {
                return true;
            }
            if (command.Contains("torrConf"))
            {
                return true;
            }
            {
                string valuePart = command.Split(command)[0].Trim();
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
