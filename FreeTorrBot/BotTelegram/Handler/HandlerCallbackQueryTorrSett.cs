using AdTorrBot.BotTelegram;
using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram.Db.Model;
using FreeTorrBot.BotTelegram;
using FreeTorrserverBot.Torrserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;
using Telegram.Bot.Types.ReplyMarkups;

namespace AdTorrBot.BotTelegram.Handler
{
    public class HandlerCallbackQueryTorrSett:MessageHandler
    {
        public static async Task HandlerTextInputMessage(Message message, TextInputFlag textInputFlag)
        {
            var text = message.Text;
            var idMessage = message.MessageId;
            var lastTextFlagTrue = textInputFlag.LastTextFlagTrue;

            Console.WriteLine($"Зашел в HandlerTextInputMessage. Последний активный флаг: {lastTextFlagTrue}");

            if (string.IsNullOrEmpty(lastTextFlagTrue))
            {
                Console.WriteLine("Нет активного режима ввода.");
                return;
            }

            // Удаляем сообщение, чтобы пользователь не видел текстового ввода
            await DeleteMessage(idMessage);

            // Обработка в зависимости от последнего активного флага
            switch (lastTextFlagTrue)
            {
                case "FlagLogin":
                    Console.WriteLine("Обработка TextInputFlagLogin");
                    if (InputTextValidator.ValidateLoginAndPassword(text))
                    {
                        await Torrserver.ChangeAccountTorrserver(text, "", true, false);
                        await SqlMethods.SwitchTorSettingsInputFlag("FlagLogin", false);
                        Console.WriteLine("Смена логина выполнена.");
                        await botClient.SendTextMessageAsync(AdminChat,
                            $"Ваш новый логин ➡️ {text} установлен ✅",
                            replyMarkup: KeyboardManager.GetDeleteThisMessage());
                    }
                    else
                    {
                        Console.WriteLine("Смена логина не удалась.");
                        await botClient.SendTextMessageAsync(AdminChat,
                            "❗ Вы в режиме ввода логина.\n" +
                            "Напишите желаемый логин.\n" +
                            "⚠️ Логин может содержать только английские буквы и цифры. Ограничение: 10 символов.",
                            replyMarkup: KeyboardManager.CreateExitTorrSettInputButton("FlagLogin"));
                    }
                    break;

                case "FlagPassword":
                    Console.WriteLine("Обработка TextInputFlagPassword");
                    if (InputTextValidator.ValidateLoginAndPassword(text))
                    {
                        await Torrserver.ChangeAccountTorrserver("", text, false, true);
                        await SqlMethods.SwitchTorSettingsInputFlag("FlagPassword", false);
                        Console.WriteLine("Смена пароля выполнена.");
                        await botClient.SendTextMessageAsync(AdminChat,
                            $"Ваш новый пароль ➡️ {text} установлен ✅",
                            replyMarkup: KeyboardManager.GetDeleteThisMessage());
                    }
                    else
                    {
                        Console.WriteLine("Смена пароля не удалась.");
                        await botClient.SendTextMessageAsync(AdminChat,
                            "❗ Вы в режиме ввода пароля.\n" +
                            "Напишите желаемый пароль.\n" +
                            "⚠️ Пароль может содержать только английские буквы и цифры. Ограничение: 10 символов.",
                            replyMarkup: KeyboardManager.CreateExitTorrSettInputButton("FlagPassword"));
                    }
                    break;

                default:
                    Console.WriteLine($"Обработка для {lastTextFlagTrue} пока не реализована.");
                    await botClient.SendTextMessageAsync(AdminChat,
                        $"Вы в режиме ввода данных для настройки: {lastTextFlagTrue}. Пока обработка не поддерживается.",
                        replyMarkup: KeyboardManager.GetDeleteThisMessage());
                    break;
            }
        }



        public static async Task CheckSettingAndExecute(CallbackQuery callbackQuery, string inputSetting)
        {
            Console.WriteLine($"Пришло в CheckSettingsAndExecute:{inputSetting}");
            var callbackData = callbackQuery.Data;
            var idMessage = callbackQuery.Message.MessageId;  // ID сообщения для редактирования

            // Прочитаем конфиг
            var conf = await Torrserver.ReadConfig();

            // Преобразуем строку в нижний регистр для удобства сравнения
            string setting = inputSetting.ToLower();

            // Определяем смайлы в переменных для удобства изменения
            string enabledSymbol = "\u2705";  // ✅
            string disabledSymbol = "\u274C"; // ❌

            switch (setting)
            {
                // Поля типа bool - переключение на противоположное состояние
                case "usedisk":
                    conf.UseDisk = !conf.UseDisk;
                    await SendOrEditMessage(idMessage, $"Использование диска теперь {(conf.UseDisk ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "enableipv6":
                    conf.EnableIPv6 = !conf.EnableIPv6;
                    await SendOrEditMessage(idMessage, $"IPv6 теперь {(conf.EnableIPv6 ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "disablestcp":
                    conf.DisableTCP = !conf.DisableTCP;
                    await SendOrEditMessage(idMessage, $"Отключение TCP теперь {(conf.DisableTCP ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "disableutp":
                    conf.DisableUTP = !conf.DisableUTP;
                    await SendOrEditMessage(idMessage, $"μTP теперь {(conf.DisableUTP ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "disablepex":
                    conf.DisablePEX = !conf.DisablePEX;
                    await SendOrEditMessage(idMessage, $"PEX теперь {(conf.DisablePEX ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "forceencrypt":
                    conf.ForceEncrypt = !conf.ForceEncrypt;
                    await SendOrEditMessage(idMessage, $"Принудительное шифрование теперь {(conf.ForceEncrypt ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "disabledht":
                    conf.DisableDHT = !conf.DisableDHT;
                    await SendOrEditMessage(idMessage, $"DHT теперь {(conf.DisableDHT ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "disableupnp":
                    conf.DisableUPNP = !conf.DisableUPNP;
                    await SendOrEditMessage(idMessage, $"UPNP теперь {(conf.DisableUPNP ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "enabledlna":
                    conf.EnableDLNA = !conf.EnableDLNA;
                    await SendOrEditMessage(idMessage, $"DLNA теперь {(conf.EnableDLNA ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "enablerutorsearch":
                    conf.EnableRutorSearch = !conf.EnableRutorSearch;
                    await SendOrEditMessage(idMessage, $"Поиск по RuTor теперь {(conf.EnableRutorSearch ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "enabledebug":
                    conf.EnableDebug = !conf.EnableDebug;
                    await SendOrEditMessage(idMessage, $"Режим отладки теперь {(conf.EnableDebug ? enabledSymbol : disabledSymbol)}",KeyboardManager.GetShoWTorrConfig());
                    break;

                case "responsivemode":
                    conf.ResponsiveMode = !conf.ResponsiveMode;
                    await SendOrEditMessage(idMessage, $"Быстрый режим чтения теперь {(conf.ResponsiveMode ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "disableupload":
                    conf.DisableUpload = !conf.DisableUpload;
                    await SendOrEditMessage(idMessage, $"Отключение отдачи теперь {(conf.DisableUpload ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                case "removecacheondrop":
                    conf.RemoveCacheOnDrop = !conf.RemoveCacheOnDrop;
                    await SendOrEditMessage(idMessage, $"Удаление кеша при сбросе теперь {(conf.RemoveCacheOnDrop ? enabledSymbol : disabledSymbol)}", KeyboardManager.GetShoWTorrConfig());
                    break;

                    // Поля, требующие ввода данных (int, long, string)
                    
                        case "cachesize":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettCacheSize", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода размера кеша. Пожалуйста, введите новое значение (MB).", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettCacheSize"));
                            break;

                        case "readerreadahead":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettReaderReadAHead", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода значения для опережающего кеша. Пожалуйста, введите новое значение.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettReaderReadAHead"));
                            break;

                        case "preloadcache":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettPreloadCache", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода размера буфера предзагрузки. Пожалуйста, введите новое значение.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettPreloadCache"));
                            break;

                        case "torrentdisconnecttimeout":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettTorrentDisconnectTimeout", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода тайм-аута отключения торрентов. Пожалуйста, введите новое значение (в секундах).", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettTorrentDisconnectTimeout"));
                            break;

                        case "connectionslimit":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettConnectionsLimit", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода лимита соединений для торрентов. Пожалуйста, введите новое значение.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettConnectionsLimit"));
                            break;

                        case "downloadratelimit":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettDownloadRateLimit", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода ограничения скорости загрузки. Пожалуйста, введите новое значение (кб/с).", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettDownloadRateLimit"));
                            break;

                        case "uploadratelimit":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettUploadRateLimit", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода ограничения скорости отдачи. Пожалуйста, введите новое значение (кб/с).", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettUploadRateLimit"));
                            break;

                        case "peerslistenport":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettPeersListenPort", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода порта для входящих подключений. Пожалуйста, введите новый порт.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettPeersListenPort"));
                            break;

                        case "retrackersmode":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettRetrackersMode", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода режима ретрекеров. Пожалуйста, введите новое значение.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettRetrackersMode"));
                            break;

                        case "sslport":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettSslPort", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода SSL порта. Пожалуйста, введите новый порт.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettSslPort"));
                            break;

                        case "friendlyname":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettFriendlyName", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода имени сервера DLNA. Пожалуйста, введите новое имя.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettFriendlyName"));
                            break;

                        case "torrentssavepath":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettTorrentsSavePath", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода пути для сохранения торрентов. Пожалуйста, введите новый путь.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettTorrentsSavePath"));
                            break;

                        case "sslcert":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettSslCert", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода пути к SSL сертификату. Пожалуйста, введите путь.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettSslCert"));
                            break;

                        case "sslkey":
                            await SqlMethods.SwitchTorSettingsInputFlag("FlagTorrSettSslKey", true);
                            await SendOrEditMessage(idMessage, "Вы в режиме ввода пути к SSL ключу. Пожалуйста, введите путь.", KeyboardManager.CreateExitTorrSettInputButton("FlagTorrSettSslKey"));
                            break;

                        default:
                            await SendOrEditMessage(idMessage, "Неизвестная настройка", KeyboardManager.buttonHideButtots);
                            break;
                    }


                    await Torrserver.WriteConfig(conf);
         
        }

        public static async Task SendOrEditMessage(int idMessage, string message, InlineKeyboardMarkup keyCallback)
        {
            await botClient.EditMessageTextAsync(AdminChat,idMessage, "\u2699 Настройки Torrserver\r\n" + message,replyMarkup: keyCallback);
            return;
        }


    }
}
