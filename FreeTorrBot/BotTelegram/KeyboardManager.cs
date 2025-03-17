using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using FreeTorrserverBot.Torrserver;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreeTorrBot.BotTelegram
{
    public abstract class KeyboardManager

    {
        public static InlineKeyboardButton buttonHideButtots = InlineKeyboardButton.WithCallbackData("❌", "deletemessages");
      //  public static InlineKeyboardButton backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 \uD83D\uDD10", "сontrolTorrserver");
        public static InlineKeyboardMarkup GetDeleteThisMessage()
        {
            var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
               {new[]{InlineKeyboardButton.WithCallbackData("❌", "deletemessages")}});
            return inlineKeyboarDeleteMessageOnluOnebutton;

        }
      
        #region BitTorrConfig
        public static async Task<InlineKeyboardMarkup> GetBitTorrConfigMain(string idChat, BitTorrConfig config, int startIndex)
        {
            // Проверка на null перед выполнением логики метода
            if (config == null)
            {
                Console.WriteLine("Ошибка: Config object is null.");
                throw new ArgumentNullException(nameof(config), "Config object is null");
            }

            int totalItems = typeof(BitTorrConfig).GetProperties().Length - 3;

            var properties = typeof(BitTorrConfig).GetProperties()
                                         .Skip(startIndex + 3)
                                         .Take(5) // Отображаем 5 свойств, начиная с переданного индекса
                                         .Select(prop =>
                                         {
                                             // Получаем описание из атрибута DescriptionAttribute
                                             var descriptionAttr = prop.GetCustomAttribute<DescriptionAttribute>();
                                             string description = descriptionAttr != null ? descriptionAttr.Description : prop.Name;

                                             // Проверка на null для значения свойства
                                             var value = prop.GetValue(config) ?? "не задано";

                                             bool isNumeric = prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long);
                                             // Устанавливаем значение для callbackData
                                             int valueCallbackData = isNumeric && value != null ? Convert.ToInt32(value) : 0;
                                             // Формируем текст кнопки с описанием и значением
                                             string buttonText = $"{description} ({value})";

                                             // Логирование для отладки [Не удалять|Для нормальной инициализации полей с set]
                                             Console.WriteLine($"Свойство {prop.Name}, значение: {value}");
                                             // Возвращаем кнопку с callback-данными
                                             return InlineKeyboardButton.WithCallbackData(buttonText, $"{valueCallbackData}torrSetOne{prop.Name}");
                                         })
                                         .ToArray();

            var keyboardButtons = new List<InlineKeyboardButton[]>();

            // Группируем кнопки по 2 в строке
            for (int i = 0; i < properties.Length; i += 1)
            {
                keyboardButtons.Add(properties.Skip(i).Take(1).ToArray());
            }

            // Добавляем кнопки "Назад" и "Вперед" для навигации, если это уместно
            var navigationButtons = new List<InlineKeyboardButton>();

            if (startIndex > 0)
            {
                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("⬅️ Назад", $"{startIndex - 5}torrSettings"));
            }
            navigationButtons.Add(InlineKeyboardButton.WithCallbackData("\u2139", "showTorrsetInfo"));
            if (startIndex + 6 < totalItems)
            {
                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("Вперед ➡️", $"{startIndex + 5}torrSettings"));
            }

            if (navigationButtons.Any())
            {
                keyboardButtons.Add(navigationButtons.ToArray());
            }

            // Добавляем кнопки "Назад" и "Скрыть" в последний ряд
            var buttonBackSettingsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var buttonResetSetBitTorrConfig = InlineKeyboardButton.WithCallbackData("\u267B", "resetTorrSetConfig");
            var buttonSetBitTorrConfig = InlineKeyboardButton.WithCallbackData("✅", "setTorrSetConfig");
            keyboardButtons.Add(new[] { buttonBackSettingsMain, buttonResetSetBitTorrConfig, buttonSetBitTorrConfig });

            return new InlineKeyboardMarkup(keyboardButtons);
        }
        public static InlineKeyboardMarkup CreateExitBitTorrConfigInputButton(string callbackData, long value)
        {
            // Кнопка выхода
            var buttonExit = InlineKeyboardButton.WithCallbackData("\uD83D\uDEAA Выход из режима ввода", "exitFlag" + callbackData);
            string tset = "torrSetOne";
            // Формируем общий массив кнопок
            var buttons = new List<InlineKeyboardButton[]>();
            // Логика добавления дополнительных кнопок
            var additionalButtons = new List<InlineKeyboardButton>();
            if (callbackData.Contains("TorrSettSslKey"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}SslKey"));
            }
            if (callbackData.Contains("TorrSettSslCert"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}SslCert"));
            }
            if (callbackData.Contains("TorrSettTorrentsSavePath"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}TorrentsSavePath"));
            }
            if (callbackData.Contains("TorrSettSslPort"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("0 (8091)", $"{0}{tset}SslPort"));
            }
            if (callbackData.Contains("TorrSettRetrackersMode"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("0", $"{0}{tset}RetrackersMode"));
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("1", $"{1}{tset}RetrackersMode"));
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("2", $"{2}{tset}RetrackersMode"));
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("3", $"{3}{tset}RetrackersMode"));

            }
            if (callbackData.Contains("TorrSettFriendlyName"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}FriendlyName"));
            }
            if (callbackData.Contains("TorrSettPeersListenPort"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("0 (авто)", $"{0}{tset}PeersListenPort"));
            }
            if (callbackData.Contains("TorrSettUploadRateLimit"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("0", $"{0}{tset}UploadRateLimit"));
            }
            if (callbackData.Contains("TorrSettDownloadRateLimit"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("0", $"{0}{tset}DownloadRateLimit"));
            }
            if (callbackData.Contains("TorrSettConnectionsLimit"))
            {

                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("25 шт.", $"{25}{tset}ConnectionsLimit"));

            }
            if (callbackData.Contains("TorrSettTorrentDisconnectTimeout"))
            {

                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("30 сек.", $"{30}{tset}TorrentDisconnectTimeout"));

            }
            if (callbackData.Contains("TorrSettCacheSize"))
            {
                //Мин 32 Макс 256
                var backValue = value - 32;
                var nextValue = value + 32;


                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("64 МБ", $"{64}{tset}CacheSize"));

                if (value >= 64)
                {
                    additionalButtons.Add(InlineKeyboardButton.WithCallbackData("-32 МБ", $"{backValue}{tset}CacheSize"));
                }
                if (value <= 224)
                {
                    additionalButtons.Add(InlineKeyboardButton.WithCallbackData("+32 МБ", $"{nextValue}{tset}CacheSize"));
                }
            }
            if (callbackData.Contains("TorrSettReaderReadAHead"))
            {
                //Мин 5 Макс 100
                var backValue = value - 5;
                var nextValue = value + 5;


                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("95 %", $"{95}{tset}ReaderReadAHead"));

                if (value >= 10)
                {
                    additionalButtons.Add(InlineKeyboardButton.WithCallbackData("-5 %", $"{backValue}{tset}ReaderReadAHead"));
                }
                if (value <= 95)
                {
                    additionalButtons.Add(InlineKeyboardButton.WithCallbackData("+5 %", $"{nextValue}{tset}ReaderReadAHead"));
                }
            }
            if (callbackData.Contains("TorrSettPreloadCache"))
            {
                //Мин 5 Макс 100
                var backValue = value - 5;
                var nextValue = value + 5;


                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("50 %", $"{50}{tset}PreloadCache"));

                if (value >= 10)
                {
                    additionalButtons.Add(InlineKeyboardButton.WithCallbackData("-5 %", $"{backValue}{tset}PreloadCache"));
                }
                if (value <= 95)
                {
                    additionalButtons.Add(InlineKeyboardButton.WithCallbackData("+5 %", $"{nextValue}{tset}PreloadCache"));
                }
            }
            if (callbackData.Contains("FlagLogin") || callbackData.Contains("FlagPassword"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("↩", "manage_login_password"));
            }
            if (callbackData.Contains("TorrSett"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("↩", "0torrSettings"));
            }


            // Добавляем дополнительные кнопки, если они есть
            if (additionalButtons.Count > 0)
            {
                buttons.Add(additionalButtons.ToArray()); // Кнопки в строку
            }

            // Добавляем кнопку выхода в отдельную строку
            buttons.Add(new[] { buttonExit });

            return new InlineKeyboardMarkup(buttons);
        }
        public static InlineKeyboardMarkup GetShoWBitTorrConfig()
        {
            var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
               {
                new[]{InlineKeyboardButton.WithCallbackData("↩", "0torrSettings")
                      ,InlineKeyboardButton.WithCallbackData("\uD83D\uDD04", "showTorrsetInfo")
                      ,InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")
                }
            });
            return inlineKeyboarDeleteMessageOnluOnebutton;

        }
        #endregion BitTorrConfig
        #region ServerArgsConfig
        public static InlineKeyboardMarkup CreateExitServerArgsConfigInputButton(string callbackData, long value)
        {
            // Кнопка выхода
            var buttonExit = InlineKeyboardButton.WithCallbackData("❌ Выход из режима ввода", "exitFlag" + callbackData);
            var buttonBack = InlineKeyboardButton.WithCallbackData("↩", "0torrArgsSettings");
            string tset = "torrConfigSetOne"; // Префикс для callback-данных

            // Формируем общий массив кнопок
            var buttons = new List<InlineKeyboardButton[]>();

            // Логика добавления дополнительных кнопок
            var additionalButtons = new List<InlineKeyboardButton>();

            if (callbackData.Contains("ServerArgsSettSslKey"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}SslKey"));
            }
            else if (callbackData.Contains("ServerArgsSettSslCert"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}SslCert"));
            }
            else if (callbackData.Contains("ServerArgsSettWebLogPath"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}WebLogPath"));
            }
            else if (callbackData.Contains("ServerArgsSettTorrentsDir"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}TorrentsDir"));
            }
            else if (callbackData.Contains("ServerArgsSettSslPort"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("(8443)", $"{8443}{tset}SslPort"));
            }
            else if (callbackData.Contains("ServerArgsSettPort"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("(8090)", $"{8090}{tset}Port"));
            }
            else if (callbackData.Contains("ServerArgsSettTorrentAddr"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}TorrentAddr"));
            }
            else if (callbackData.Contains("ServerArgsSettPubIPv4"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}PubIPv4"));
            }
            else if (callbackData.Contains("ServerArgsSettPubIPv6"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}PubIPv6"));
            }
            else if (callbackData.Contains("ServerArgsSettPath"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}Path"));
            }
            else if (callbackData.Contains("ServerArgsSettLogPath"))
            {
                additionalButtons.Add(InlineKeyboardButton.WithCallbackData("По умолчанию", $"{1}{tset}LogPath"));
            }

            // Если callbackData не содержит специфической логики, оставляем пустой if
            else
            {
                // Пустой блок для других случаев
            }

            // Добавляем дополнительные кнопки, если они есть
            if (additionalButtons.Count > 0)
            {
                buttons.Add(additionalButtons.ToArray()); // Кнопки в строку
            }

            // Добавляем кнопку выхода в отдельную строку
            buttons.Add(new[] { buttonExit, buttonBack });

            return new InlineKeyboardMarkup(buttons);
        }

        

        public static InlineKeyboardMarkup GetShoWServerArgsConfig()
        {
            var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
               {
                new[]{InlineKeyboardButton.WithCallbackData("↩", "0torrArgsSettings")
                      ,InlineKeyboardButton.WithCallbackData("\uD83D\uDD04", "showTorrArgssetInfo")
                      ,InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")
                }
            });
            return inlineKeyboarDeleteMessageOnluOnebutton;

        }
        public static InlineKeyboardMarkup GetServerArgsConfigMain(string idChat, ServerArgsConfig config, int startIndex)
        {
            // Проверка на null перед выполнением логики метода
            if (config == null)
            {
                Console.WriteLine("Ошибка: Config object is null.");
                throw new ArgumentNullException(nameof(config), "Config object is null");
            }
            Console.WriteLine($"Свойство Port, значение: {config.Port}");

            int totalItems = typeof(ServerArgsConfig).GetProperties().Length - 3;
            Console.WriteLine($"Вывод 5 свойств конфига args.");
            var properties = typeof(ServerArgsConfig).GetProperties()
                                         .Skip(startIndex + 3)
                                         .Take(5) // Отображаем 5 свойств, начиная с переданного индекса
                                         .Select(prop =>
                                         {
                                             // Получаем описание из атрибута DescriptionAttribute
                                             var descriptionAttr = prop.GetCustomAttribute<DescriptionAttribute>();
                                             string description = descriptionAttr != null ? descriptionAttr.Description : prop.Name;

                                             // Проверка на null для значения свойства
                                             var value = prop.GetValue(config) ?? "не задано";

                                             bool isNumeric = prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long);
                                             // Устанавливаем значение для callbackData
                                             int valueCallbackData = isNumeric && value != null ? Convert.ToInt32(value) : 0;
                                             // Формируем текст кнопки с описанием и значением
                                             string buttonText = $"{description} ({value})";

                                             // Логирование для отладки [Не удалять|Для нормальной инициализации полей с set]
                                             Console.WriteLine($"Свойство {prop.Name}, значение: {value}");
                                             // Возвращаем кнопку с callback-данными
                                             return InlineKeyboardButton.WithCallbackData(buttonText, $"{valueCallbackData}torrConfigSetOne{prop.Name}");
                                         })
                                         .ToArray();

            var keyboardButtons = new List<InlineKeyboardButton[]>();

            // Группируем кнопки по 2 в строке
            for (int i = 0; i < properties.Length; i += 1)
            {
                keyboardButtons.Add(properties.Skip(i).Take(1).ToArray());
            }

            // Добавляем кнопки "Назад" и "Вперед" для навигации, если это уместно
            var navigationButtons = new List<InlineKeyboardButton>();

            if (startIndex > 0)
            {
                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("⬅️ Назад", $"{startIndex - 5}torrArgsSettings"));
            }
            navigationButtons.Add(InlineKeyboardButton.WithCallbackData("\u2139", "showTorrArgssetInfo"));
            if (startIndex + 6 < totalItems)
            {
                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("Вперед ➡️", $"{startIndex + 5}torrArgsSettings"));
            }

            if (navigationButtons.Any())
            {
                keyboardButtons.Add(navigationButtons.ToArray());
            }

            // Добавляем кнопки "Назад" и "Скрыть" в последний ряд
            var buttonBackSettingsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var buttonResetSetArgsTorrConfig = InlineKeyboardButton.WithCallbackData("\u267B", "resetTorrArgsSetConfig");
            var buttonSetArgsTorrConfig = InlineKeyboardButton.WithCallbackData("✅", "setTorrArgsSetConfig");
            keyboardButtons.Add(new[] { buttonBackSettingsMain, buttonResetSetArgsTorrConfig, buttonSetArgsTorrConfig });

            return new InlineKeyboardMarkup(keyboardButtons);
        }
        public static InlineKeyboardMarkup GetRestartingMain()
        {
            var restartTorrServer = InlineKeyboardButton.WithCallbackData("🔄 Перезагрузка Torr", "restart_torrserver");
            var restartServer = InlineKeyboardButton.WithCallbackData("🔄 Перезагрузка Сервера", "restart_server");
            var inlineRestartingMain = new InlineKeyboardMarkup(new[]
            {
                new[]{restartServer, restartTorrServer }
                ,new[]{buttonHideButtots}

            });
            return inlineRestartingMain;

        }
        #endregion ServerArgsConfig
        #region MainMenu
        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            var butGuardMenu = new KeyboardButton("\uD83D\uDD10 Доступ");
            var butBackupMenu = new KeyboardButton("\uD83D\uDCBE Бекапы");
            var butRestartingMenu = new KeyboardButton("🔄 Перезагрузки");
            var butSettinsTorrserver = new KeyboardButton("⚙ Настройки");
            // var butSettinsBot = new KeyboardButton("\u2699 Настройки бота");

            // var buttonSettings = InlineKeyboardButton.WithCallbackData("⚙️ Настройки", "settings");
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { butGuardMenu,butBackupMenu},
        new[] { butRestartingMenu,butSettinsTorrserver }
            })
            {
                ResizeKeyboard = true // Это сделает клавиатуру более компактной
            };
        }
        #region Settings
        public static InlineKeyboardMarkup GetSettingsMain()
        {
            var setTorrSettings = InlineKeyboardButton.WithCallbackData("⚙️ Настройки Torrsever", "0torrSettings");
            var setTorrConfig = InlineKeyboardButton.WithCallbackData("🛠️ Конфиг Torrsever", "0torrArgsSettings");
            var setServer = InlineKeyboardButton.WithCallbackData("💻 Настройки сервера", "set_server");
            var setBot = InlineKeyboardButton.WithCallbackData("🤖 Настройки бота", "set_bot");
            var inlineSettingsMain = new InlineKeyboardMarkup(new[]
            {
            new[] {setTorrSettings,setTorrConfig},
            new[] {setServer,setBot}
            ,new[] {buttonHideButtots}
            });
            return inlineSettingsMain;
        }

        public static InlineKeyboardMarkup GetMainTimeZone()
        {

            var buttonLeftTime = InlineKeyboardButton.WithCallbackData("\u2B05", "-time_zone");
            var buttonRightTime = InlineKeyboardButton.WithCallbackData("\u27A1", "+time_zone");
            var buttonBackSettingsBot = InlineKeyboardButton.WithCallbackData("\u21A9", "set_bot");
            var inlineTimeZoneMain = new InlineKeyboardMarkup(new[]
            {
                new[]{buttonLeftTime, buttonRightTime},
                new[] {buttonBackSettingsBot,buttonHideButtots}
            });
            return inlineTimeZoneMain;
        }
        public static InlineKeyboardMarkup GetSettingsBot()
        {
            var buttonTimeZone = InlineKeyboardButton.WithCallbackData("\uD83C\uDF0F Часовой пояс", "time_zone");
            var buttonBackSettinsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var inlineSettinsBotMenu = new InlineKeyboardMarkup(new[]
            {
                new[]{buttonTimeZone}
                ,new[]{buttonBackSettinsMain,buttonHideButtots}
            });
            return inlineSettinsBotMenu;
        }
        public static InlineKeyboardMarkup GetSetServerBbrMain(bool isActiv)
        {
            var setServerBbrButton = isActiv
                ? InlineKeyboardButton.WithCallbackData("Выкл", "0set_server_bbr")
                : InlineKeyboardButton.WithCallbackData("Вкл", "1set_server_bbr");

            var backSetServer = InlineKeyboardButton.WithCallbackData("↩", "set_server");
            var inlineSetServerMain = new InlineKeyboardMarkup(new[]
                    {
                new[] { setServerBbrButton },
                new[] { backSetServer, buttonHideButtots }
            });

            return inlineSetServerMain;
        }

        public static InlineKeyboardMarkup GetSetServerMain()
        {
            var setServerBbr = InlineKeyboardButton.WithCallbackData("Bbr", "set_server_bbr");
            var buttonBackSettinsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var inlineSetServerMain = new InlineKeyboardMarkup(new[]
            {
                new[] {setServerBbr}
                ,new[] {buttonBackSettinsMain, buttonHideButtots}

            });
            return inlineSetServerMain;
        }
        #endregion Settings
        #region Backups
        public static InlineKeyboardMarkup GetMainBackups()
        {
            var butBackupTorrserver = InlineKeyboardButton.WithCallbackData("\u2601 \u26A1 Бекап torrserver ", "backupTorr");
            var butBackupBotSettings = InlineKeyboardButton.WithCallbackData("\u2601 \u2699 Бекап настроек бота", "backupBotSettings");
            var inlineBackupMenu = new InlineKeyboardMarkup(new[]
           {
                 new[]{butBackupTorrserver }
                 ,new[] { butBackupBotSettings }
                 , new[] {buttonHideButtots}
            });
            return inlineBackupMenu;
        }
        #endregion Backups
        #region Доступ
        public static InlineKeyboardMarkup GetSetTimeAutoChangePassword()
        {
            var butHourBack = InlineKeyboardButton.WithCallbackData("- 1 час", "-60setAutoPassMinutes");
            var butHourNext = InlineKeyboardButton.WithCallbackData("+ 1 час", "+60setAutoPassMinutes");
            var butMinuteBack = InlineKeyboardButton.WithCallbackData("- 10 мин.", "-10setAutoPassMinutes");
            var butMinuteNext = InlineKeyboardButton.WithCallbackData("+ 10 мин.", "+10setAutoPassMinutes");
            var backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 ", "сontrolTorrserver");

            var inlineSetAutoChangePass = new InlineKeyboardMarkup(new[]
            {
               new[]{butHourBack,butHourNext}
               ,new[] {butMinuteBack,butMinuteNext}
               ,new[]{backGetControlTorrserver,buttonHideButtots}
            });
            return inlineSetAutoChangePass;
        }
        public static InlineKeyboardMarkup GetNewLoginPasswordMain()
        {

            // Кнопка для генерации нового логина и пароля
            var buttonGenerateNewPassword = InlineKeyboardButton.WithCallbackData("🔑 🔄 Пароль", "generate_new_password");

            // Кнопка для ручного ввода логина и пароля
            var buttonSetPasswordManually = InlineKeyboardButton.WithCallbackData("🔑 ✍️ Пароль", "set_password_manually");

            // Кнопка для генерации нового логина
            var buttonGenerateNewLogin = InlineKeyboardButton.WithCallbackData("👤 🔄 Логин", "generate_new_login");

            // Кнопка для ручного ввода логина
            var buttonSetLoginManually = InlineKeyboardButton.WithCallbackData("👤 ✍️ Логин", "set_login_manually");
            var backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 ", "сontrolTorrserver");

            var buttonShowLoginPassword = InlineKeyboardButton.WithCallbackData("👀 Показать логин и пароль", "show_login_password");
            // Возврат InlineKeyboardMarkup с кнопками
            return new InlineKeyboardMarkup(new[]
            {
                new[] {  buttonSetPasswordManually,buttonGenerateNewPassword},
                new[] {  buttonSetLoginManually, buttonGenerateNewLogin },

                new[] { buttonShowLoginPassword  },
                new[] {backGetControlTorrserver,buttonHideButtots}
                });

        }

        public static InlineKeyboardMarkup GetProfilesUsersTorrserver()
        {
            var buttonMainProfile = InlineKeyboardButton.WithCallbackData("Главный профиль 🏠", "MainProfile");
            var buttonOtherProfiles = InlineKeyboardButton.WithCallbackData("Другие профили 👥", "0OtherProfilessort_active");

            return new InlineKeyboardMarkup(new[]
            {
                        new[] {buttonMainProfile},
                        new[] { buttonOtherProfiles },
                        new[] { buttonHideButtots }
            });
        }
        public static InlineKeyboardMarkup CreateNewProfileTorrserverUser()
        {
          
            var buttonBack = InlineKeyboardButton.WithCallbackData("↩️", "0OtherProfilessort_active");

            return new InlineKeyboardMarkup(new[]
            {
                        new[] {buttonBack,buttonHideButtots},
            });
        }
        public static InlineKeyboardMarkup GetControlOtherProfilesTorrserver(int nextCount, int allCount, string sort)
        {
            var buttonCreateProfile = InlineKeyboardButton.WithCallbackData("👤 Создать профиль", "createNewProfile");
            var buttonBackMenu = InlineKeyboardButton.WithCallbackData("↩️", "BackProfilesUersTorrserver");

            // Логика для кнопок "Назад" и "Далее"
            InlineKeyboardButton? buttonBack = null;
            InlineKeyboardButton? buttonNext = null;
            if (nextCount >5)
            {
                // Если есть куда листать назад (сдвиг хотя бы на одну страницу)
                buttonBack = InlineKeyboardButton.WithCallbackData("⬅️", $"{nextCount - 10}OtherProfiles{sort}");
            }

            if (nextCount <= allCount)
            {
                buttonNext = InlineKeyboardButton.WithCallbackData("➡️", $"{nextCount}OtherProfiles{sort}");
            }



            // Логика для кнопок сортировки
            InlineKeyboardButton? buttonSortActive = null;
            InlineKeyboardButton? buttonSortInActive = null;
            InlineKeyboardButton? buttonSortDateEnd = null;

            // Список для кнопок сортировки на одной строке
            var sortButtons = new List<InlineKeyboardButton>();

            if (sort != "sort_active")
            {
                sortButtons.Add(InlineKeyboardButton.WithCallbackData("🟢🔽", $"0OtherProfilessort_active"));
            }

            if (sort != "sort_inactive")
            {
                sortButtons.Add(InlineKeyboardButton.WithCallbackData("🔴🔽", $"0OtherProfilessort_inactive"));
            }

            if (sort != "sort_date")
            {
                sortButtons.Add(InlineKeyboardButton.WithCallbackData("📅🔚", $"0OtherProfilessort_date"));
            }
            // Формирование клавиатуры
            var buttons = new List<InlineKeyboardButton[]>
                {
                    new[] { buttonCreateProfile } // Первая строка
                };
            // Добавляем кнопки сортировки в одну строку, если есть хотя бы одна
            if (sortButtons.Count > 0)
            {
                buttons.Add(sortButtons.ToArray());
            }
            // Добавляем строку с кнопками "Назад" и "Далее", если они доступны
            var navigationButtons = new List<InlineKeyboardButton>();
               navigationButtons?.Add(buttonBackMenu);
            if (buttonBack != null) navigationButtons?.Add(buttonBack);
            if (buttonNext != null) navigationButtons?.Add(buttonNext);
            navigationButtons?.Add(buttonHideButtots);
            if (navigationButtons?.Count > 0) buttons.Add(navigationButtons.ToArray());

         

            return new InlineKeyboardMarkup(buttons);
        }

        public static InlineKeyboardMarkup GetControlTorrserver()
        {
            // var buttonChangeLogin = InlineKeyboardButton.WithCallbackData("👤 \u2699 (new)Логин ", "change_login");
            // var buttonPrintLogin = InlineKeyboardButton.WithCallbackData(" 👀 Логин ", "print_login");
            // var buttonChangePassword = InlineKeyboardButton.WithCallbackData("🔑 \u2699 (new)Пароль", "change_password");
            // var buttonPrintPassword = InlineKeyboardButton.WithCallbackData("👀  Пароль", "print_password");
            var buttonManageLoginPassword = InlineKeyboardButton.WithCallbackData("👤🔑 Управление логином и паролем", "manage_login_password");
            var buttonChangeTimeAuto = InlineKeyboardButton.WithCallbackData("⏰ Автосмена 🔑", "change_time_auto");
            var buttonPrintTimeAuto = InlineKeyboardButton.WithCallbackData("👀 Автосмена 🔑", "print_time_auto");
            var buttonEnableAutoChange = InlineKeyboardButton.WithCallbackData("✅ Вкл. Автосмену 🔑", "enable_auto_change");
            var buttonDisableAutoChange = InlineKeyboardButton.WithCallbackData("❌ Откл. Автосмену 🔑", "disable_auto_change");
            var buttonUpdateGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\uD83D\uDD04", "сontrolTorrserver");
            var buttonBack = InlineKeyboardButton.WithCallbackData("↩️", "BackProfilesUersTorrserver");
            // var buttonShowStatus = InlineKeyboardButton.WithCallbackData("📊 Текущее состояние", "show_status");

            return new InlineKeyboardMarkup(new[]
            {
                        new[] {buttonManageLoginPassword},
                        new[] { buttonChangeTimeAuto, buttonPrintTimeAuto },
                        new[] { buttonEnableAutoChange, buttonDisableAutoChange },
                        new[] { buttonBack,buttonUpdateGetControlTorrserver, buttonHideButtots}
            });
        }

        #endregion Доступ
        #endregion MainMenu



        //public static InlineKeyboardMarkup GetSettingsTorrserver() {}

  




    }
}
