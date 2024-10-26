using AdTorrBot.BotTelegram.Db.Model;
using FreeTorrserverBot.BotTelegram;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
                setTorr.TimeAutoChangePassword = ParsingMethods.UpdateTimeString(timeStringAutoChangePassTorrNow, minutes);
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            }
            );

        }
        public static async Task<TextInputFlag> GetTextInputFlag(string idChat)
        {
            return await WithDbContextAsync(async db =>
            {
                var textInputFlags = db.TextInputFlag.FirstOrDefault(x => x.IdChat == idChat);
               
                return textInputFlags;
            });
        }
        public static async Task<bool> IsTextInputFlagLogin(string idChat)
        {
            Console.WriteLine("Заход в IsTextInputFlagLogin");

            // Выполняем и получаем результат из WithDbContextAsync
            var result = await WithDbContextAsync(async db =>
            {
                var textInputFlags = await db.TextInputFlag.FirstOrDefaultAsync(x => x.IdChat == idChat);
                var flagLogin = textInputFlags?.FlagLogin ?? false;
                Console.WriteLine($"flaglogin:{flagLogin}");

                return flagLogin;
            });

            return result;

        }
        public static async Task SwitchTextInputFlagLogin(string idChat,bool value)
        {
            await WithDbContextAsync(async db =>
            {
                var textInputFlags = db.TextInputFlag.FirstOrDefault(x => x.IdChat == idChat);
                textInputFlags.FlagLogin=value;
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            });
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
                // Убедимся, что таблицы созданы
                await db.Database.EnsureCreatedAsync();
                var existingSettingsBot = await db.SettingsBot.FirstOrDefaultAsync(s => s.IdChat == idChat);
                var existingSettingsTorrserver = await db.SettingsTorrserverBot.FirstOrDefaultAsync(s => s.idChat == idChat);
                var existingUser = await db.User.FirstOrDefaultAsync(u => u.IdChat == idChat);
                var existingTextInputFlags = await db.TextInputFlag.FirstOrDefaultAsync(u => u.IdChat == idChat);
                var entitiesToAdd = new List<object>();
                if (existingTextInputFlags == null)
                {
                    entitiesToAdd.Add(new TextInputFlag { IdChat = idChat });
                }
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
