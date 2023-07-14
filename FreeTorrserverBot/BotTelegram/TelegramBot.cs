using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using System.IO;
using IniParser.Model;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreeTorrserverBot.BotTelegram
{
    public  class TelegramBot
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
        private static FileIniDataParser parser = new FileIniDataParser();
        private static IniData data = parser.ReadFile(path);
        static private TelegramBotClient client = new TelegramBotClient(data["Profile0"]["YourBotTelegreamToken"]);

        public static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
           
            var Message = update.Message;
            var ChatId = update?.CallbackQuery?.Message?.Chat?.Id.ToString();
            var InputText = Message?.Text;
            var InlineText = update?.CallbackQuery?.Data;
            var buttonChangePassword = new KeyboardButton("Поменять пароль");
            var buttonPrintPassword = new KeyboardButton("Посмотреть пароль");
            var keyboardMain = new ReplyKeyboardMarkup(new[]{ buttonChangePassword, buttonPrintPassword });
            var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                 {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});
            keyboardMain.ResizeKeyboard= true;
            if(update?.CallbackQuery?.Data!=null)
            {

                if(InlineText== "deletemessages")
                {
                    await botClient.DeleteMessageAsync(ChatId, update.CallbackQuery.Message.MessageId);
                    return;
                }    
            }
            if (Message?.Text != null)
            {
                ChatId = Message.Chat.Id.ToString();
                if (InputText=="Поменять пароль")
                {
                    await botClient.DeleteMessageAsync(ChatId, Message.MessageId);
                     Torrserver.ChangeAccountTorrserver();
                    await botClient.SendTextMessageAsync(ChatId
                                                         , "Пароль успешно изменен !"
                                                         , replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
                    return;
                }
                if(InputText=="Посмотреть пароль")
                {
                    await botClient.DeleteMessageAsync(ChatId, Message.MessageId);
                    var newParol = Torrserver.TakeAccountTorrserver();
                    await botClient.SendTextMessageAsync(ChatId
                                                       , $"{newParol}"
                                                       , replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton) ;
                    return;
                }    
                if(InputText=="/start")
                {
                    await botClient.DeleteMessageAsync(ChatId, Message.MessageId);
                    await botClient.SendTextMessageAsync(ChatId
                                                         ,"Бот по смене паполя Torrserver приветствует тебя"
                                                         , replyMarkup:keyboardMain);
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
        static public async Task  StartBot()
        {
            var statrBotThread = new Thread(() => client.StartReceiving(Update, Error));
            statrBotThread.Start();


            Console.ReadLine();
        }
    }
}
