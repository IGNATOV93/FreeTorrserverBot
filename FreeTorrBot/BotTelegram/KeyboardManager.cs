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
             var  inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
            return inlineKeyboarDeleteMessageOnluOnebutton;

        }
        public static InlineKeyboardMarkup GetMainBackups()
        {
            var butBackupTorrserver = InlineKeyboardButton.WithCallbackData("backupTorr", "\u2601 \u26A1 Бекап torrserver ");
            var butBackupBotSettings = InlineKeyboardButton.WithCallbackData("backupBotSettings", "\u2601 \u2699 Бекап настроек бота");
            var inlineBackupMenu = new InlineKeyboardMarkup(new[]
           {
                 new[]{butBackupTorrserver, butBackupBotSettings, buttonHideButtots}
            });
            return inlineBackupMenu;
        }
        public static InlineKeyboardMarkup GetSetTimeAutoChangePassword()
        {
            var butHourBack = InlineKeyboardButton.WithCallbackData("- 1 час", "-60setAutoPassMinutes");
            var butHourNext = InlineKeyboardButton.WithCallbackData("+ 1 час", "+60setAutoPassMinutes");
            var butMinuteBack = InlineKeyboardButton.WithCallbackData("- 10 мин.", "-10setAutoPasMinutes");
            var butMinuteNext = InlineKeyboardButton.WithCallbackData("+ 10 мин.", "+10setAutoPasMinutes");
           var backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 ", "сontrolTorrserver");
            var inlineSetAutoChangePass = new InlineKeyboardMarkup(new[]
            {
               new[]{butHourBack,butHourNext}
               ,new[] {butMinuteBack,butMinuteNext}
                ,new[] {backGetControlTorrserver,buttonHideButtots}
            });
            return inlineSetAutoChangePass;
        }
        public static InlineKeyboardMarkup GetControlTorrserver()
        {
            var buttonChangeLogin = InlineKeyboardButton.WithCallbackData("👤 \u2699 логин ", "change_login");
            var buttonPrintLogin = InlineKeyboardButton.WithCallbackData(" 👀 Логин ", "print_login");
            var buttonChangePassword = InlineKeyboardButton.WithCallbackData("🔑 \u2699 пароль", "change_password");
            var buttonPrintPassword = InlineKeyboardButton.WithCallbackData("👀  Пароль", "print_password");
            var buttonChangeTimeAuto = InlineKeyboardButton.WithCallbackData("⏰ Автосмена 🔑", "change_time_auto");
            var buttonPrintTimeAuto = InlineKeyboardButton.WithCallbackData("👀 Автосмена 🔑", "print_time_auto");
            var buttonEnableAutoChange = InlineKeyboardButton.WithCallbackData("✅ Вкл. Автосмену 🔑", "enable_auto_change");
            var buttonDisableAutoChange = InlineKeyboardButton.WithCallbackData("❌ Откл. Автосмену 🔑", "disable_auto_change");
            var buttonUpdateGetControlTorrserver = backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\uD83D\uDD04", "сontrolTorrserver");
           // var buttonShowStatus = InlineKeyboardButton.WithCallbackData("📊 Текущее состояние", "show_status");

            return new InlineKeyboardMarkup(new[]
            {
                        new[] {buttonChangeLogin, buttonPrintLogin},
                        new[] { buttonChangePassword, buttonPrintPassword },
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
