using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static InlineKeyboardMarkup GetSetServerBbrMain() 
            {
            var onSetServerBbr = InlineKeyboardButton.WithCallbackData("Вкл", "1set_server_bbr");
            var offSetServerBbr = InlineKeyboardButton.WithCallbackData("Выкл", "0set_server_bbr");
            var backSetServerBbr = InlineKeyboardButton.WithCallbackData("↩", "set_server_bbr");
            var inlineSetServerMain = new InlineKeyboardMarkup(new[]
            {
                new[] {onSetServerBbr,offSetServerBbr}
                ,new[] {backSetServerBbr,buttonHideButtots}

            });
            return inlineSetServerMain;
        }
        public static InlineKeyboardMarkup GetSetServerMain()
        {
            var setServerBbr = InlineKeyboardButton.WithCallbackData("Bbr","set_server_bbr");
            var buttonBackSettinsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var inlineSetServerMain = new InlineKeyboardMarkup(new[]
            {
                new[] {setServerBbr,buttonBackSettinsMain}

            } );
            return inlineSetServerMain;
        }
        public static InlineKeyboardMarkup GetTorrSettingsMain() 
            {
            var buttonBackSettinsMain = InlineKeyboardButton.WithCallbackData("↩", "back_settings_main");
            var inlineTorrSettingsMain = new InlineKeyboardMarkup(new[]
          {
                new[]{buttonBackSettinsMain, buttonHideButtots }
            });
            return inlineTorrSettingsMain;
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

            var setTorrSettings = InlineKeyboardButton.WithCallbackData("⚙️ Настройки Torrsever", "torr_settings");
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
        new[] { butSettinsTorrserver }
            })
            {
                ResizeKeyboard = true // Это сделает клавиатуру более компактной
            };
        }

    }
}
