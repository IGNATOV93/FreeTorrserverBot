using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using FreeTorrserverBot.BotTelegram;

namespace FreeTorrserverBot
{
    public  class MyRegistry : Registry
    {
        static string TimeAutoChangePassworTorrserver = TelegramBot.configuration["Profile0:TimeAutoChangePassword"];
        public MyRegistry()
        {
            int hours = int.Parse(TimeAutoChangePassworTorrserver.Split(":")[0]);
            int minutes = int.Parse(TimeAutoChangePassworTorrserver.Split(":")[1]);
            Schedule(async () => await Torrserver.Torrserver.AutoChangeAccountTorrserver())
                 .ToRunEvery(1)
                 .Days()
                 .At(hours, minutes); 


        }
    }
}
