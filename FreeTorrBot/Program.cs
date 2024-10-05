using System;
using System.Collections.Generic;
using System.Linq;
using FluentScheduler;
using FreeTorrserverBot.BotTelegram;
using FreeTorrserverBot.Torrserver;

public class MainClass
{
    public static TimeZoneInfo moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
    public static DateTime nowTimeMsk = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, moscowTimeZone);
    static async Task Main()
    {
        await TelegramBot.StartBot();

        Console.ReadLine();
    }
}