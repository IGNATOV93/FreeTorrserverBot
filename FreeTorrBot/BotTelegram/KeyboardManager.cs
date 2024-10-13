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
        public static InlineKeyboardMarkup GetDeleteThisMessage()
        {
             var  inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
            return inlineKeyboarDeleteMessageOnluOnebutton;

        }
       
        public static InlineKeyboardMarkup GetControlTorrserver()
        {
            var buttonChangePassword = InlineKeyboardButton.WithCallbackData("🔑 Поменять пароль", "change_password");
            var buttonPrintPassword = InlineKeyboardButton.WithCallbackData("👀 Посмотреть пароль", "print_password");
            var buttonChangeTimeAuto = InlineKeyboardButton.WithCallbackData("⏰ Время автосмены", "change_time_auto");
            var buttonPrintTimeAuto = InlineKeyboardButton.WithCallbackData("🕒 Посмотреть время", "print_time_auto");
            var buttonEnableAutoChange = InlineKeyboardButton.WithCallbackData("✅ Включить автосмену", "enable_auto_change");
            var buttonDisableAutoChange = InlineKeyboardButton.WithCallbackData("❌ Отключить автосмену", "disable_auto_change");
            var buttonShowStatus = InlineKeyboardButton.WithCallbackData("📊 Текущее состояние", "show_status");
            var buttonHideButtots = InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages");
            return new InlineKeyboardMarkup(new[]
            {
            new[] { buttonChangePassword, buttonPrintPassword },
            new[] { buttonChangeTimeAuto, buttonPrintTimeAuto },
            new[] { buttonEnableAutoChange, buttonDisableAutoChange },
            new[] { buttonShowStatus,buttonHideButtots}
        });
        }

        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            var buttonAdminMenu = new KeyboardButton("🛠 Управление");
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
