using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram;
using FluentScheduler;
using FreeTorrserverBot.BotTelegram;
using AdTorrBot.BotTelegram.Db.Model;


namespace FreeTorrserverBot
{
    public  class MyRegistry : Registry
    {
        public MyRegistry()
        {
            ScheduleJobAsync().GetAwaiter().GetResult();
        }

        private async Task ScheduleJobAsync()
        {
            int hours = await GetHoursAsync();
            int minutes = await GetMinutesAsync();

            Schedule(async () => await Torrserver.Torrserver.AutoChangeAccountTorrserver())
                .ToRunEvery(1)
                .Days()
                .At(hours, minutes);
        }

        private async Task<int> GetHoursAsync()
        {
            // Предполагается асинхронная загрузка данных
            var settings = await LoadSettingsAsync();
            return int.Parse(settings.TimeAutoChangePassword.Split(":")[0]);
        }

        private async Task<int> GetMinutesAsync()
        {
            // Предполагается асинхронная загрузка данных
            var settings = await LoadSettingsAsync();
            return int.Parse(settings.TimeAutoChangePassword.Split(":")[1]);
        }
        private async Task<SettingsTorrserverBot> LoadSettingsAsync()
        {
            
          return  await SqlMethods.GetSettingsTorrserverBot(BotTelegram.TelegramBot.AdminChat);
            // Ваша логика для загрузки настроек
        }
    }
}
