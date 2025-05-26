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
            var isTorrserverAutoRunEnabled = GetTorrserverAutoRunSettingAsync().GetAwaiter().GetResult();
            if (isTorrserverAutoRunEnabled)
            {
                ScheduleTorrserverAutoRun().GetAwaiter().GetResult(); // Автозапуск именно Torrserver
            }
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
         //  Console.WriteLine($"Итоговое время автосмены пароля на самом сервере : {finalHours:D2}:{finalMinutes:D2}"); // Формат с ведущим нулем

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
        private async Task<bool> GetTorrserverAutoRunSettingAsync()
        {
            var settings = await SqlMethods.GetSettingsTorrserverBot();
            return settings.IsTorrserverAutoRunEnabled; // Новый флаг в базе данных
        }

        private async Task ScheduleTorrserverAutoRun()
        {
            var setBot = await SqlMethods.GetSettingBot();
            var timeZone = ServerInfo.GetLocalServerTimeTimeZone();
            double hoursWithTimeZone = timeZone - setBot.TimeZoneOffset;

            int hours = await GetTorrserverHoursAsync();
            int minutes = await GetTorrserverMinutesAsync();

            // Корректируем с учетом часового пояса
            int additionalHours = (int)hoursWithTimeZone;
            int additionalMinutes = (int)((hoursWithTimeZone - additionalHours) * 60);

            int finalHours = hours + additionalHours;
            int finalMinutes = minutes + additionalMinutes;

            // Если минуты превышают 60, корректируем
            if (finalMinutes >= 60)
            {
                finalHours += finalMinutes / 60;
                finalMinutes %= 60;
            }

            Schedule(async () => await RunTorrserverTask())
                .ToRunEvery(1)
                .Days()
                .At(finalHours, finalMinutes);

         //   Console.WriteLine($"✅ Автозапуск Torrserver запланирован на {finalHours:D2}:{finalMinutes:D2} (с учетом часового пояса)");
        }

        private async Task RunTorrserverTask()
        {
            await Torrserver.Torrserver.RebootingTorrserver();
            await BotTelegram.TelegramBot.SendMessageToAdmin("✅Ежедневный авто-перезапуск Torrserver выполнен!")
            // Логика автозапуска Torrserver
            Console.WriteLine("Ежедневный авто-перезапуск Torrserver выполнен!");
            // Здесь можно добавить вызов нужного метода
        }

        private async Task<int> GetTorrserverHoursAsync()
        {
            var settings = await LoadSettingsAsync();
            return int.Parse(settings.TorrserverTaskTime.Split(":")[0]); // Время из базы
        }

        private async Task<int> GetTorrserverMinutesAsync()
        {
            var settings = await LoadSettingsAsync();
            return int.Parse(settings.TorrserverTaskTime.Split(":")[1]); // Время из базы
        }

        private async Task<SettingsTorrserverBot> LoadSettingsAsync()
        {
            return await SqlMethods.GetSettingsTorrserverBot();
        }
    }
}
