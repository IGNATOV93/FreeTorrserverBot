using FreeTorrBot.BotTelegram;
using FreeTorrserverBot.Torrserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AdTorrBot.BotTelegram.Handler
{
    public class HandlerCallbackQueryTorrSett:MessageHandler
    {

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
                    await SendOrEditMessage(idMessage, $"Использование диска теперь {(conf.UseDisk ? enabledSymbol : disabledSymbol)}");
                    break;

                case "enableipv6":
                    conf.EnableIPv6 = !conf.EnableIPv6;
                    await SendOrEditMessage(idMessage, $"IPv6 теперь {(conf.EnableIPv6 ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disablestcp":
                    conf.DisableTCP = !conf.DisableTCP;
                    await SendOrEditMessage(idMessage, $"Отключение TCP теперь {(conf.DisableTCP ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disableutp":
                    conf.DisableUTP = !conf.DisableUTP;
                    await SendOrEditMessage(idMessage, $"μTP теперь {(conf.DisableUTP ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disablepex":
                    conf.DisablePEX = !conf.DisablePEX;
                    await SendOrEditMessage(idMessage, $"PEX теперь {(conf.DisablePEX ? enabledSymbol : disabledSymbol)}");
                    break;

                case "forceencrypt":
                    conf.ForceEncrypt = !conf.ForceEncrypt;
                    await SendOrEditMessage(idMessage, $"Принудительное шифрование теперь {(conf.ForceEncrypt ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disabledht":
                    conf.DisableDHT = !conf.DisableDHT;
                    await SendOrEditMessage(idMessage, $"DHT теперь {(conf.DisableDHT ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disableupnp":
                    conf.DisableUPNP = !conf.DisableUPNP;
                    await SendOrEditMessage(idMessage, $"UPNP теперь {(conf.DisableUPNP ? enabledSymbol : disabledSymbol)}");
                    break;

                case "enabledlna":
                    conf.EnableDLNA = !conf.EnableDLNA;
                    await SendOrEditMessage(idMessage, $"DLNA теперь {(conf.EnableDLNA ? enabledSymbol : disabledSymbol)}");
                    break;

                case "enablerutorsearch":
                    conf.EnableRutorSearch = !conf.EnableRutorSearch;
                    await SendOrEditMessage(idMessage, $"Поиск по RuTor теперь {(conf.EnableRutorSearch ? enabledSymbol : disabledSymbol)}");
                    break;

                case "enabledebug":
                    conf.EnableDebug = !conf.EnableDebug;
                    await SendOrEditMessage(idMessage, $"Режим отладки теперь {(conf.EnableDebug ? enabledSymbol : disabledSymbol)}");
                    break;

                case "responsivemode":
                    conf.ResponsiveMode = !conf.ResponsiveMode;
                    await SendOrEditMessage(idMessage, $"Быстрый режим чтения теперь {(conf.ResponsiveMode ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disableupload":
                    conf.DisableUpload = !conf.DisableUpload;
                    await SendOrEditMessage(idMessage, $"Отключение отдачи теперь {(conf.DisableUpload ? enabledSymbol : disabledSymbol)}");
                    break;

                case "removecacheondrop":
                    conf.RemoveCacheOnDrop = !conf.RemoveCacheOnDrop;
                    await SendOrEditMessage(idMessage, $"Удаление кеша при сбросе теперь {(conf.RemoveCacheOnDrop ? enabledSymbol : disabledSymbol)}");
                    break;

                // Поля, требующие ввода данных (int, long, string)
                case "cachesize":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода размера кеша. Пожалуйста, введите новое значение (MB).");
                    break;

                case "readerreadahead":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода значения для опережающего кеша. Пожалуйста, введите новое значение.");
                    break;

                case "preloadcache":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода размера буфера предзагрузки. Пожалуйста, введите новое значение.");
                    break;

                case "torrentdisconnecttimeout":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода тайм-аута отключения торрентов. Пожалуйста, введите новое значение (в секундах).");
                    break;

                case "connectionslimit":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода лимита соединений для торрентов. Пожалуйста, введите новое значение.");
                    break;

                case "downloadratelimit":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода ограничения скорости загрузки. Пожалуйста, введите новое значение (кб/с).");
                    break;

                case "uploadratelimit":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода ограничения скорости отдачи. Пожалуйста, введите новое значение (кб/с).");
                    break;

                case "peerslistenport":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода порта для входящих подключений. Пожалуйста, введите новый порт.");
                    break;

                case "retrackersmode":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода режима ретрекеров. Пожалуйста, введите новое значение.");
                    break;

                case "sslport":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода SSL порта. Пожалуйста, введите новый порт.");
                    break;

                case "nameserver":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода имени сервера DLNA. Пожалуйста, введите новое имя.");
                    break;

                case "torrentssavepath":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути для сохранения торрентов. Пожалуйста, введите новый путь.");
                    break;

                case "friendlyname":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода имени сервера DLNA. Пожалуйста, введите новое имя.");
                    break;

                case "sslcert":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути к SSL сертификату. Пожалуйста, введите путь.");
                    break;

                case "sslkey":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути к SSL ключу. Пожалуйста, введите путь.");
                    break;

                default:
                    await SendOrEditMessage(idMessage, "Неизвестная настройка");
                    break;
            }

            await Torrserver.WriteConfig(conf);
         
        }

        public static async Task SendOrEditMessage(int idMessage, string message)
        {
            await botClient.EditMessageTextAsync(AdminChat,idMessage, "\u2699 Настройки Torrserver\r\n" + message,replyMarkup:KeyboardManager.GetShoWTorrConfig());
            return;
        }


    }
}
