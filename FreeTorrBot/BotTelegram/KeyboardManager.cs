using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreeTorrBot.BotTelegram
{
    public abstract class KeyboardManager

    {
        public static InlineKeyboardButton buttonHideButtots = InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages");
        public static InlineKeyboardButton backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 \uD83D\uDD10", "сontrolTorrserver");
        public static InlineKeyboardMarkup GetDeleteThisMessage()
        {
            var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
               {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
            return inlineKeyboarDeleteMessageOnluOnebutton;

        }
        public static InlineKeyboardMarkup GetShoWTorrConfig()
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
        public static async Task<InlineKeyboardMarkup> GetBitTorrConfigMain(string idChat,BitTorrConfig config, int startIndex)
        {
            
            int totalItems = typeof(BitTorrConfig).GetProperties().Length-3;

            var properties = typeof(BitTorrConfig).GetProperties()
                                           .Skip(startIndex+3)
                                           .Take(5) // Отображаем 5 свойств, начиная с переданного индекса
                                           .Select(prop => InlineKeyboardButton.WithCallbackData($"{config.GetDescription(prop.Name)}", $"torrSetOne{prop.Name}"))
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
                // Добавляем кнопку "Назад", если не находимся в начале списка
                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("⬅️ Назад", $"{startIndex - 5}torrSettings"));
            }
            navigationButtons.Add(InlineKeyboardButton.WithCallbackData("\u2139", "showTorrsetInfo"));
            if (startIndex + 6 < totalItems)
            {
                // Добавляем кнопку "Вперед", если не достигнут конец списка
                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("Вперед ➡️", $"{startIndex + 5}torrSettings"));
            }

            // Добавляем кнопки навигации в клавиатуру, если они есть
            if (navigationButtons.Any())
            {
                keyboardButtons.Add(navigationButtons.ToArray());
            }

            // Добавляем кнопки "Назад" и "Скрыть" в последний ряд
            var buttonBackSettingsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var buttonResetSetBitTorrConfig = InlineKeyboardButton.WithCallbackData("\u267B", "resetTorrSetConfig");
            var buttonSetBitTorrConfig = InlineKeyboardButton.WithCallbackData("✅", "setTorrSetConfig");
            keyboardButtons.Add(new[] { buttonBackSettingsMain, buttonResetSetBitTorrConfig, buttonSetBitTorrConfig, buttonHideButtots });

            return new InlineKeyboardMarkup(keyboardButtons);
        }

        public static InlineKeyboardMarkup ExitTextPassword()
        {
            var buttonExitTextPassword = InlineKeyboardButton.WithCallbackData("\uD83D\uDEAA Выход из режима ввода пароля ", "exitTextPassword");
            var inlineExitTextPasswoed = new InlineKeyboardMarkup(new[]
           {
                 new[]{ buttonExitTextPassword }
            });
            return inlineExitTextPasswoed;
        }
        public static InlineKeyboardMarkup ExitTextLogin()
        {
            var buttonExitTextLogin = InlineKeyboardButton.WithCallbackData("\uD83D\uDEAA Выход из режима ввода логина ","exitTextLogin");
            var inlineExitTextLogin= new InlineKeyboardMarkup(new[]
           {
                 new[]{ buttonExitTextLogin }
            });
            return inlineExitTextLogin;
        }
        public static InlineKeyboardMarkup GetMainBackups()
        {
            var butBackupTorrserver = InlineKeyboardButton.WithCallbackData("\u2601 \u26A1 Бекап torrserver ","backupTorr");
            var butBackupBotSettings = InlineKeyboardButton.WithCallbackData("\u2601 \u2699 Бекап настроек бота","backupBotSettings" );
            var inlineBackupMenu = new InlineKeyboardMarkup(new[]
           {
                 new[]{butBackupTorrserver }
                 ,new[] { butBackupBotSettings }
                 , new[] {buttonHideButtots}
            });
            return inlineBackupMenu;
        }
     
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
            var buttonUpdateGetControlTorrserver = backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\uD83D\uDD04", "сontrolTorrserver");
           // var buttonShowStatus = InlineKeyboardButton.WithCallbackData("📊 Текущее состояние", "show_status");

            return new InlineKeyboardMarkup(new[]
            {
                        new[] {buttonManageLoginPassword},
                        new[] { buttonChangeTimeAuto, buttonPrintTimeAuto },
                        new[] { buttonEnableAutoChange, buttonDisableAutoChange },
                        new[] { buttonUpdateGetControlTorrserver, buttonHideButtots}
            });
        }
    
        //public static InlineKeyboardMarkup GetSettingsTorrserver() {}
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
            var buttonTimeZone = InlineKeyboardButton.WithCallbackData("\uD83C\uDF0F Часовой пояс","time_zone");
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
            var setServerBbr = InlineKeyboardButton.WithCallbackData("Bbr","set_server_bbr");
            var buttonBackSettinsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var inlineSetServerMain = new InlineKeyboardMarkup(new[]
            {
                new[] {setServerBbr}
                ,new[] {buttonBackSettinsMain, buttonHideButtots}

            } );
            return inlineSetServerMain;
        }

        public static InlineKeyboardMarkup GetTorrConfigMain()
        {
            var buttonBackSettinsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var inlineTorrConfigMain = new InlineKeyboardMarkup(new[]
          {
                new[]{buttonBackSettinsMain, buttonHideButtots }
            });
            return inlineTorrConfigMain;
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
        public static InlineKeyboardMarkup GetSettingsMain()
        {

            var setTorrSettings = InlineKeyboardButton.WithCallbackData("⚙️ Настройки Torrsever", "0torrSettings");
            var setTorrConfig = InlineKeyboardButton.WithCallbackData("🛠️ Конфиг Torrsever", "torr_config");
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

    }
}
