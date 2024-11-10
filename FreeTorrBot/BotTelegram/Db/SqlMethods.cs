using AdTorrBot.BotTelegram.Db.Model;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using FreeTorrserverBot.BotTelegram;
using FreeTorrserverBot.Torrserver;
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

        public static async Task SetSettingsTorrProfile(string idChat, BitTorrConfig config)
        {
            try
            {
                // Используем метод для работы с базой данных
                await SqlMethods.WithDbContextAsync(async db =>
                {
                    // Ищем профиль по idChat
                    var existingProfile = db.BitTorrConfig.FirstOrDefault(x => x.IdChat == idChat);

                    if (existingProfile != null)
                    {
                        // Используем SetValues, но исключаем Id
                        var currentValues = db.Entry(existingProfile).CurrentValues;
                        var configValues = db.Entry(config).CurrentValues;

                        // Копируем все значения, кроме Id
                        foreach (var property in currentValues.Properties)
                        {
                            if (property.Name != nameof(existingProfile.Id))
                            {
                                currentValues[property.Name] = configValues[property.Name];
                            }
                        }

                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        // Если профиль не найден, создаем новый
                        config.IdChat = idChat; // Присваиваем idChat для нового профиля
                        db.BitTorrConfig.Add(config);
                        await db.SaveChangesAsync();
                    }
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // Логируем ошибку, если что-то пошло не так
                Console.WriteLine($"Ошибка при сохранении профиля в базу данных: {ex.Message}");
            }
        }

        public static async Task<BitTorrConfig> GetSettingsTorrProfile(string idChat)
        {
            BitTorrConfig updatedProfile = new BitTorrConfig() { IdChat=idChat};

            await SqlMethods.WithDbContextAsync(async db =>
            {
                // Пытаемся найти профиль по IdChat
                var existingProfile = db.BitTorrConfig.FirstOrDefault(x => x.IdChat == idChat);

                if (existingProfile != null)
                {
                    // Если профиль найден, обновляем его
                    var newProfile = await Torrserver.ReadConfig();
                    newProfile.IdChat = idChat;
                    newProfile.Id=existingProfile.Id;

                    // Обновляем все поля, используя SetValues
                    db.Entry(existingProfile).CurrentValues.SetValues(newProfile);
                    await db.SaveChangesAsync();

                    updatedProfile = existingProfile;  // Возвращаем обновленный профиль
                }
                else
                {
                    // Если профиль не найден, создаем новый
                    var newProfile = await Torrserver.ReadConfig();
                    newProfile.IdChat = idChat;

                    db.BitTorrConfig.Add(newProfile);
                    await db.SaveChangesAsync();

                    updatedProfile = newProfile;  // Возвращаем новый профиль
                }
                return Task.CompletedTask;
            });

            return updatedProfile;
        }


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

        public static async Task SwitchTimeZone(string idChat, string indicator)
        {
            await WithDbContextAsync(async db =>
            {
                var setBot = db.SettingsBot.FirstOrDefault(x => x.IdChat == idChat);
                setBot.ChangeTimeZone(indicator);
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            });
        }
        public static async Task SwitchTextInputFlagPassword(string idChat, bool value)
        {
            await WithDbContextAsync(async db =>
            {
                var textInputFlags = db.TextInputFlag.FirstOrDefault(x => x.IdChat == idChat);
                textInputFlags.FlagPassword = value;
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            });
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
        public static async Task SetLoginPasswordSettingsTorrserverBot(string idChat,string login,string password)
        {
            await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsTorrserverBot.FirstOrDefault(x => x.idChat == idChat);
                setTorr.Login = login;
                setTorr.Password = password;
              await  db.SaveChangesAsync();
                return Task.CompletedTask;
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
                // Применяем все необходимые миграции
                await db.Database.MigrateAsync();

                // Убедимся, что таблицы созданы
              //  await db.Database.EnsureCreatedAsync();
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
