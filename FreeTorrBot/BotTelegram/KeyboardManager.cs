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
        public static InlineKeyboardMarkup GetBackControlTorrserver()
        {
            var backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 ", "сontrolTorrserver");
            var inlineBackControlTorrserver = new InlineKeyboardMarkup(new[]
            {
                new[]{ backGetControlTorrserver,buttonHideButtots}
            });
            return inlineBackControlTorrserver;
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
            var buttonGenerateNewLoginPassword = InlineKeyboardButton.WithCallbackData("👤 🔄 Сгенерировать пароль", "generate_new_password");

            // Кнопка для ручного ввода логина и пароля
            var buttonSetLoginPasswordManually = InlineKeyboardButton.WithCallbackData("👤 ✍️ Ввести пароль", "set_password_manually");

            // Кнопка для генерации нового логина
            var buttonGenerateNewLogin = InlineKeyboardButton.WithCallbackData("👤 🔄 Сгенерировать логин", "generate_new_login");

            // Кнопка для ручного ввода логина
            var buttonSetLoginManually = InlineKeyboardButton.WithCallbackData("👤 ✍️ Ввести логин", "set_login_manually");
            var backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 ", "сontrolTorrserver");

            var buttonShowLoginPassword = InlineKeyboardButton.WithCallbackData("👀 Показать логин и пароль", "show_login_password");
            // Возврат InlineKeyboardMarkup с кнопками
            return new InlineKeyboardMarkup(new[]
            {
                new[] { buttonGenerateNewLoginPassword },
                new[] { buttonSetLoginPasswordManually },
                new[] { buttonGenerateNewLogin },
                new[] { buttonSetLoginManually },
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
    
        //public static InlineKeyboardMarkup GetSettingsTorrserver() {        }
        public static InlineKeyboardMarkup GetSettingsBot()
        {
            var buttonTimeZone = InlineKeyboardButton.WithCallbackData("\uD83C\uDF0F Часовой пояс");
            var inlineSettinsBotMenu = new InlineKeyboardMarkup(new[]
            {
                new[]{buttonTimeZone, buttonHideButtots }
            });
            return inlineSettinsBotMenu;
        }
        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            var butGuardMenu = new KeyboardButton("\uD83D\uDD10 Доступ");
            var butBackupMenu = new KeyboardButton("\uD83D\uDCBE Бекапы");
            var butSettinsTorrserver = new KeyboardButton("⚙ Настройки Torrserver");
            var butSettinsBot = new KeyboardButton("\u2699 Настройки бота");
            
           // var buttonSettings = InlineKeyboardButton.WithCallbackData("⚙️ Настройки", "settings");
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { butGuardMenu,butBackupMenu},
        new[] { butSettinsTorrserver,butSettinsBot }
            })
            {
                ResizeKeyboard = true // Это сделает клавиатуру более компактной
            };
        }

    }
}
