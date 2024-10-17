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
        public static InlineKeyboardMarkup GetDeleteThisMessage()
        {
             var  inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
            return inlineKeyboarDeleteMessageOnluOnebutton;

        }

        public static InlineKeyboardMarkup GetSetTimeAutoChangePassword()
        {
            var butHourBack = InlineKeyboardButton.WithCallbackData("- 1 час", "backHourPassAuto");
            var butHourNext = InlineKeyboardButton.WithCallbackData("+ 1 час", "nextHourPassAuto");
            var butMinuteBack = InlineKeyboardButton.WithCallbackData("- 10 мин.", "backMinutePassAuto");
            var butMinuteNext = InlineKeyboardButton.WithCallbackData("+ 10 мин.", "nextMitutePassAuto");
            var backGetControlTorrserver = InlineKeyboardButton.WithCallbackData("\u21A9 \uD83D\uDD10", "сontrolTorrserver");
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
           // var buttonShowStatus = InlineKeyboardButton.WithCallbackData("📊 Текущее состояние", "show_status");
           
            return new InlineKeyboardMarkup(new[]
            {
                        new[] {buttonChangeLogin, buttonPrintLogin},
                        new[] { buttonChangePassword, buttonPrintPassword },
                        new[] { buttonChangeTimeAuto, buttonPrintTimeAuto },
                        new[] { buttonEnableAutoChange, buttonDisableAutoChange },
                        new[] {buttonHideButtots}
            });
        }

        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            var buttonAdminMenu = new KeyboardButton("\uD83D\uDD10 Доступ");
           
           // var buttonSettings = InlineKeyboardButton.WithCallbackData("⚙️ Настройки", "settings");
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { buttonAdminMenu }
    })
            {
                ResizeKeyboard = true // Это сделает клавиатуру более компактной
            };
        }

    }
}
