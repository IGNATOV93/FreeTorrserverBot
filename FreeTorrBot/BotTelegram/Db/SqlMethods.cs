using AdTorrBot.BotTelegram.Db.Model;
using FreeTorrserverBot.BotTelegram;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace AdTorrBot.BotTelegram.Db
{
   public abstract class SqlMethods
    {
     
        public static async Task SetTimeAutoChangePasswordTorrserver(int minutes)
        {
            await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsTorrserverBot.FirstOrDefault(x => x.idChat == TelegramBot.AdminChat);
                var timeStringAutoChangePassTorrNow = setTorr.TimeAutoChangePassword;
                setTorr.TimeAutoChangePassword = ParsingCallbackMethods.UpdateTimeString(timeStringAutoChangePassTorrNow, minutes);
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            }
            );

        }
        public static async Task SwitchAutoChangePassTorrserver(bool isActive,string idChat)
        {
           await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsTorrserverBot.FirstOrDefault(x => x.idChat == idChat);
                setTorr.IsActiveAutoChange = isActive;
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            });
            
        }
        public static async Task<SettingsBot> GetSettingBot(string idChat)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsBot.FirstOrDefault(x => x.IdChat == idChat);
                return setTorr;
            });

        }
        public static async Task<SettingsTorrserverBot> GetSettingsTorrserverBot(string idChat)
        {
         return   await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsTorrserverBot.FirstOrDefault(x => x.idChat == idChat);
                return setTorr;
            });
           
        }
        public static async Task CheckAndInsertDefaultData(string idChat)
        {
            await SqlMethods.WithDbContextAsync(async db =>
            {
                var existingSettingsBot = await db.SettingsBot.FirstOrDefaultAsync(s => s.IdChat == idChat);
                var existingSettingsTorrserver = await db.SettingsTorrserverBot.FirstOrDefaultAsync(s => s.idChat == idChat);
                var existingUser = await db.User.FirstOrDefaultAsync(u => u.IdChat == idChat);

                var entitiesToAdd = new List<object>();

                if (existingSettingsBot == null)
                    entitiesToAdd.Add(new SettingsBot { IdChat = idChat });

                if (existingSettingsTorrserver == null)
                    entitiesToAdd.Add(new SettingsTorrserverBot { idChat = idChat });

                if (existingUser == null)
                    entitiesToAdd.Add(new Model.User { IdChat = idChat });

                if (entitiesToAdd.Any())
                {
                    db.AddRange(entitiesToAdd);
                    await db.SaveChangesAsync();
                }

               
                return Task.CompletedTask;
            });
        }



        public static async Task ListTablesAsync()
        {
            await SqlMethods.WithDbContextAsync(async db =>
            {
                var entityTypes = db.Model.GetEntityTypes();
                Console.WriteLine("Список созданных таблиц в бд ");
                foreach (var entityType in entityTypes)
                {
                    Console.WriteLine(entityType.ClrType.Name); // Имя таблицы
                }
                return Task.CompletedTask; // Возвращаем завершённую задачу
            });
        }
        public static async Task<TResult> WithDbContextAsync<TResult>(Func<AppDbContext,Task<TResult>> func)
        {
            await using var db = new AppDbContext();
            return await func(db);
        }
    }
}
