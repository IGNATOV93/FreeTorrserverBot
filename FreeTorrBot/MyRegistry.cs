using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram;
using FluentScheduler;
using FreeTorrserverBot.BotTelegram;
using AdTorrBot.ServerManagement;
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
           var setBot = await SqlMethods.GetSettingBot();
            var timeZone = ServerInfo.GetLocalServerTimeTimeZone();
            double hoursWithTimeZone = timeZone-setBot.TimeZoneOffset;
            int hours = await GetHoursAsync();
            int minutes = await GetMinutesAsync();
            // Извлекаем целую часть и дробную часть
            int additionalHours = (int)hoursWithTimeZone; 
            int additionalMinutes = (int)((hoursWithTimeZone - additionalHours) * 60); 

            // Складываем часы и минуты
            int finalHours = hours + additionalHours; 
            int finalMinutes = minutes + additionalMinutes; 

            // Если минуты превышают 60, корректируем
            if (finalMinutes >= 60)
            {
                finalHours += finalMinutes / 60; // Добавляем полные часы
                finalMinutes %= 60; // Оставляем остаток минут
            }

            // Выводим итоговое время
            Console.WriteLine($"Итоговое время автосмены пароля на самом сервере : {finalHours:D2}:{finalMinutes:D2}"); // Формат с ведущим нулем

            Schedule(async () => await Torrserver.Torrserver.AutoChangeAccountTorrserver())
                .ToRunEvery(1)
                .Days()
                .At(finalHours, finalMinutes);
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
            
          return  await SqlMethods.GetSettingsTorrserverBot();
            // Ваша логика для загрузки настроек
        }
    }
}
