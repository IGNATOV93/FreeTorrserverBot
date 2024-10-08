using FluentScheduler;
using FreeTorrBot.BotTelegram.BotSettings;
using FreeTorrBot.BotTelegram.BotSettings.Model;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreeTorrserverBot.BotTelegram
{
    public class TelegramBot
    {
        static public BotSettingsJson settingsJson =  BotSettingsMethods.LoadSettings();

        static public TelegramBotClient client = new TelegramBotClient(settingsJson.YourBotTelegramToken);
        public static string AdminChat = settingsJson.AdminChatId;
        
      public static  InlineKeyboardMarkup inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
        public static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            
            var Message = update.Message;
            var ChatId = update?.CallbackQuery?.Message?.Chat?.Id.ToString();
            var InputText = Message?.Text;
            var InlineText = update?.CallbackQuery?.Data;
            var buttonChangePassword = new KeyboardButton("🔑 Поменять пароль");
            var buttonPrintPassword = new KeyboardButton("👀 Посмотреть пароль");
            var buttonChangeTimeAuto = new KeyboardButton("⏰ Время автосмены");
            var buttonPrintTimeAuto = new KeyboardButton("🕒 Посмотреть время");
            var buttonEnableAutoChange = new KeyboardButton("✅ Включить автосмену");
            var buttonDisableAutoChange = new KeyboardButton("❌ Отключить автосмену");
            var buttonShowStatus = new KeyboardButton("📊 Текущее состояние");
            var keyboardMain = new ReplyKeyboardMarkup(new[]{ new[] { buttonChangePassword, buttonPrintPassword }
                                                       , new[]{ buttonChangeTimeAuto, buttonPrintTimeAuto }
                                                       ,new[]{buttonEnableAutoChange, buttonDisableAutoChange }
                                                       ,new[] { buttonShowStatus}
                                                       });
            
            keyboardMain.ResizeKeyboard = true;
            if (update?.CallbackQuery?.Data != null)
            {
                if (ChatId != AdminChat) { return; }
                if (InlineText == "deletemessages")
                {
                    try
                    {
                        await botClient.DeleteMessageAsync(ChatId, update.CallbackQuery.Message.MessageId);
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    return;
                }
            }
            if (Message?.Text != null)
            {
                ChatId = Message.Chat.Id.ToString();
                if (ChatId != AdminChat) { return; }

                if(InlineText == "✅ Включить автосмену")
                {

                }
                if(InlineText=="❌ Отключить автосмену")
                {

                }
                if(InlineText== "📊 Текущее состояние")
                {

                }
                if (InlineText == "⏰ Время автосмены")
                {

                }
                if(InlineText =="🕒 Посмотреть время")
                {

                }
                if (InputText == "🔑 Поменять пароль")
                {
                    try 
                    {
                        await botClient.DeleteMessageAsync(ChatId, Message.MessageId);
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                   await Torrserver.Torrserver.ChangeAccountTorrserver();
                    await botClient.SendTextMessageAsync(ChatId
                                                         , "Пароль успешно изменен !"
                                                         , replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
                    return;
                }
                if (InputText == "👀 Посмотреть пароль")
                {
                    try
                    {
                        await botClient.DeleteMessageAsync(ChatId, Message.MessageId);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    var newParol =  Torrserver.Torrserver.TakeAccountTorrserver();
                    await botClient.SendTextMessageAsync(ChatId
                                                       , $"{newParol}"
                                                       , replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
                    return;
                }
                if (InputText == "/start")
                {
                    try
                    {
                        await botClient.DeleteMessageAsync(ChatId, Message.MessageId);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    await botClient.SendTextMessageAsync(ChatId
                                                         , "Бот по смене пароля Torrserver приветствует тебя !"
                                                         , replyMarkup: keyboardMain);
                    return;
                }
                return;
            }
            return;

        }
        public static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            Console.WriteLine(arg2.Message);
            return Task.CompletedTask;
        }
        static public async Task StartBot()
        {
            JobManager.Initialize(new MyRegistry());

            // Запускаем получение обновлений
            var statrBotTask = Task.Run(() => client.StartReceiving(Update, Error));

            // Ждем небольшую паузу, чтобы бот успел инициализироваться
            await Task.Delay(1000); // Настройте время ожидания по необходимости

            // Отправляем сообщение админу
            await SendMessageToAdmin("Бот успешно стартовал!");

            Console.ReadLine();
        }
        public static async Task SendMessageToAdmin(string mes)
        {
            Console.WriteLine(mes);
            try
            {
                await client.SendTextMessageAsync(AdminChat, mes, replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
            return;
        }
    }
}
