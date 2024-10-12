using FluentScheduler;
using FreeTorrBot.BotTelegram;
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

        static readonly public TelegramBotClient client = new TelegramBotClient(settingsJson.YourBotTelegramToken);
        public readonly static string AdminChat = settingsJson.AdminChatId;
        
      public static  InlineKeyboardMarkup inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
        public static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            
            var Message = update.Message;
            var ChatId = update?.CallbackQuery?.Message?.Chat?.Id.ToString();
            var InputText = Message?.Text;
            var InlineText = update?.CallbackQuery?.Data;
            if (update?.CallbackQuery?.Data != null)
            {
                if (ChatId != AdminChat&&!MessageHandler.IsCallbackQueryCommandBot(InlineText)) { return; }
                await MessageHandler.HandleUpdate(update);
                
                    try
                    {
                        await botClient.DeleteMessageAsync(AdminChat, update.CallbackQuery.Message.MessageId);
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return;
                
            }
            if (Message?.Text != null)
            {
                ChatId = Message.Chat.Id.ToString();
                if (ChatId != AdminChat&&!MessageHandler.IsTextCommandBot(InputText)) { return; }

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
