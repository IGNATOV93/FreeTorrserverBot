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
                    bool oldUseDisk = conf.UseDisk;
                    conf.UseDisk = !oldUseDisk; // Переключаем значение
                    await SendOrEditMessage(idMessage, $"Использование диска теперь {(conf.UseDisk ? enabledSymbol : disabledSymbol)}");
                    break;

                case "enableipv6":
                    bool oldEnableIPv6 = conf.EnableIPv6;
                    conf.EnableIPv6 = !oldEnableIPv6;
                    await SendOrEditMessage(idMessage, $"IPv6 теперь {(conf.EnableIPv6 ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disablestcp":
                    bool oldDisableTCP = conf.DisableTCP;
                    conf.DisableTCP = !oldDisableTCP;
                    await SendOrEditMessage(idMessage, $"Отключение TCP теперь {(conf.DisableTCP ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disableutp":
                    bool oldDisableUTP = conf.DisableUTP;
                    conf.DisableUTP = !oldDisableUTP;
                    await SendOrEditMessage(idMessage, $"μTP теперь {(conf.DisableUTP ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disablepex":
                    bool oldDisablePEX = conf.DisablePEX;
                    conf.DisablePEX = !oldDisablePEX;
                    await SendOrEditMessage(idMessage, $"PEX теперь {(conf.DisablePEX ? enabledSymbol : disabledSymbol)}");
                    break;

                case "forceencrypt":
                    bool oldForceEncrypt = conf.ForceEncrypt;
                    conf.ForceEncrypt = !oldForceEncrypt;
                    await SendOrEditMessage(idMessage, $"Принудительное шифрование теперь {(conf.ForceEncrypt ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disabledht":
                    bool oldDisableDHT = conf.DisableDHT;
                    conf.DisableDHT = !oldDisableDHT;
                    await SendOrEditMessage(idMessage, $"DHT теперь {(conf.DisableDHT ? enabledSymbol : disabledSymbol)}");
                    break;

                case "disableupnp":
                    bool oldDisableUPNP = conf.DisableUPNP;
                    conf.DisableUPNP = !oldDisableUPNP;
                    await SendOrEditMessage(idMessage, $"UPNP теперь {(conf.DisableUPNP ? enabledSymbol : disabledSymbol)}");
                    break;

                case "enabledlna":
                    bool oldEnableDLNA = conf.EnableDLNA;
                    conf.EnableDLNA = !oldEnableDLNA;
                    await SendOrEditMessage(idMessage, $"DLNA теперь {(conf.EnableDLNA ? enabledSymbol : disabledSymbol)}");
                    break;

                case "enabledebug":
                    bool oldEnableDebug = conf.EnableDebug;
                    conf.EnableDebug = !oldEnableDebug;
                    await SendOrEditMessage(idMessage, $"Режим отладки теперь {(conf.EnableDebug ? enabledSymbol : disabledSymbol)}");
                    break;

                case "responsivemode":
                    bool oldResponsiveMode = conf.ResponsiveMode;
                    conf.ResponsiveMode = !oldResponsiveMode;
                    await SendOrEditMessage(idMessage, $"Быстрый режим чтения теперь {(conf.ResponsiveMode ? enabledSymbol : disabledSymbol)}");
                    break;

                // Поля, требующие ввода данных
                case "nameserver":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода имени сервера DLNA. Пожалуйста, введите новое имя.");
                    break;

                case "torrentspath":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути для сохранения торрентов. Пожалуйста, введите новый путь.");
                    break;

                case "sslport":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода SSL порта. Пожалуйста, введите новый порт.");
                    break;

                case "sslcert":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути к SSL сертификату. Пожалуйста, введите путь.");
                    break;

                case "sslkey":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути к SSL ключу. Пожалуйста, введите путь.");
                    break;

                case "cachepath":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути к кешу. Пожалуйста, введите путь.");
                    break;

                case "peerslistenport":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода порта для входящих подключений. Пожалуйста, введите новый порт.");
                    break;

                case "downloadratelimit":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода ограничения скорости загрузки. Пожалуйста, введите новое значение (кб/с).");
                    break;

                case "uploadratelimit":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода ограничения скорости отдачи. Пожалуйста, введите новое значение (кб/с).");
                    break;

                case "torrentssavepath":
                    await SendOrEditMessage(idMessage, "Вы в режиме ввода пути для сохранения торрентов. Пожалуйста, введите новый путь.");
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
