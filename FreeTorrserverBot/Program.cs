using System;
using System.Collections.Generic;
using System.Linq;
using FreeTorrserverBot.BotTelegram;
public class MainClass
{
    static async Task Main()
    {
       await TelegramBot.StartBot();
        Console.ReadLine();
    }
}