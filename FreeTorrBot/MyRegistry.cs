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
       // static string TimeAutoChangePassworTorrserver = TelegramBot.settingsJson.TimeAutoChangePassword;
        public MyRegistry()
        {
          // int hours = int.Parse(TimeAutoChangePassworTorrserver.Split(":")[0]);
            //int minutes = int.Parse(TimeAutoChangePassworTorrserver.Split(":")[1]);
            Schedule(async () => await Torrserver.Torrserver.AutoChangeAccountTorrserver())
                 .ToRunEvery(1)
                 .Days()
                 .At(GetHours(), GetMinutes());

        }

        private int GetHours()
        {
            // Читаем время каждый раз
            string timeAutoChangePassword = TelegramBot.settingsJson.TimeAutoChangePassword;
            return int.Parse(timeAutoChangePassword.Split(":")[0]);
        }

        private int GetMinutes()
        {
            // Читаем время каждый раз
            string timeAutoChangePassword = TelegramBot.settingsJson.TimeAutoChangePassword;
            return int.Parse(timeAutoChangePassword.Split(":")[1]);
        }
    }
}
