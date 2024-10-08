﻿using FluentScheduler;

using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreeTorrserverBot.BotTelegram
{
    public class TelegramBot
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");

        // Создание конфигурации из .ini файла
        public static IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddIniFile(path)
            .Build();

        static public TelegramBotClient client = new TelegramBotClient(configuration["Profile0:YourBotTelegramToken"]);
        public static string AdminChat = configuration["Profile0:AdminChatId"];


        public static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            
            var Message = update.Message;
            var ChatId = update?.CallbackQuery?.Message?.Chat?.Id.ToString();
            var InputText = Message?.Text;
            var InlineText = update?.CallbackQuery?.Data;
            var buttonChangePassword = new KeyboardButton("Поменять пароль");
            var buttonPrintPassword = new KeyboardButton("Посмотреть пароль");
            var keyboardMain = new ReplyKeyboardMarkup(new[] { buttonChangePassword, buttonPrintPassword });
            var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
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
                if (InputText == "Поменять пароль")
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
                if (InputText == "Посмотреть пароль")
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
            var statrBotThread = new Thread(() => client.StartReceiving(Update, Error));
            statrBotThread.Start();
            
            Console.ReadLine();
        }
    }
}
