using AdTorrBot.BotTelegram.Db.Model;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using FreeTorrserverBot.BotTelegram;
using FreeTorrserverBot.Torrserver;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdTorrBot.BotTelegram.Db
{
   public abstract class SqlMethods
    {

        public static readonly string adminChat = TelegramBot.AdminChat;


       
        public static async Task SetSettingsTorrProfile( BitTorrConfig config)
        {
            Console.WriteLine($"Запуск SetSettingsTorrProfile.");
            try
            {
                // Используем метод для работы с базой данных
                await SqlMethods.WithDbContextAsync(async db =>
                {
                    // Проверяем, существует ли профиль с таким же IdChat и NameProfileBot
                    var existingProfile = await db.BitTorrConfig
                                                  .FirstOrDefaultAsync(x => x.IdChat == adminChat && x.NameProfileBot == config.NameProfileBot);

                    if (existingProfile != null)
                    {
                        // Если профиль найден с таким же IdChat и NameProfileBot, обновляем его
                        Console.WriteLine($"Профиль adminChat = {adminChat} и NameProfileBot = {existingProfile?.NameProfileBot} найден, обновляем профиль.");

                        existingProfile = config;

                   

                        await db.SaveChangesAsync();
                        Console.WriteLine("Профиль обновлен успешно.");
                    }
                    else
                    {
                        // Проверяем, существует ли профиль с таким IdChat, но другим NameProfileBot
                        var profileWithSameIdChat = await db.BitTorrConfig
                                                             .FirstOrDefaultAsync(x => x.IdChat == adminChat);

                        if (profileWithSameIdChat != null)
                        {
                            Console.WriteLine($"Профиль adminChat = {adminChat} уже существует, но с другим NameProfileBot.");
                            // Если профиль существует с таким IdChat, но с другим NameProfileBot.
                           
                        }

                        // Если профиль с таким IdChat и NameProfileBot не найден, создаем новый
                        Console.WriteLine($"Профиль adminChat = {adminChat} и NameProfileBot = {profileWithSameIdChat?.NameProfileBot} не найден, создаем новый.");
                        config.IdChat = adminChat; // Присваиваем idChat для нового профиля
                        db.BitTorrConfig.Add(config);
                        await db.SaveChangesAsync();
                        Console.WriteLine("Новый профиль добавлен в базу данных.");
                    }
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Ошибка при сохранении профиля в базу данных: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Stack trace: {ex.InnerException.StackTrace}");
                }
            }
        }



        public static async Task<BitTorrConfig> GetSettingsTorrProfile(string idChat)
        {
            Console.WriteLine($"Запуск GetSettingsTorrProfile.");
            BitTorrConfig updatedProfile = null;

            try
            {
                await SqlMethods.WithDbContextAsync(async db =>
                {
                    // Пытаемся найти профиль по IdChat
                    var existingProfile = await db.BitTorrConfig.FirstOrDefaultAsync(x => x.IdChat == idChat);

                    if (existingProfile != null)
                    {
                        Console.WriteLine($"Профиль  найден adminChat = {idChat}");
                    }
                    else
                    {
                        Console.WriteLine($"Профиль adminChat = {idChat} не найден.");
                    }

                    // Логируем чтение нового профиля
                    BitTorrConfig newProfile = await Torrserver.ReadConfig();
                    if (newProfile != null)
                    {
                        Console.WriteLine("Конфигурация прочитана успешно.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Конфигурация не была прочитана.");
                    }

                    newProfile.IdChat = idChat; // Устанавливаем idChat для нового или обновленного профиля

                    if (existingProfile != null)
                    {
                        // Если профиль найден, обновляем его
                        Console.WriteLine("Обновляем профиль.");
                        foreach (var property in typeof(BitTorrConfig).GetProperties())
                        {
                            // Игнорируем свойство Id
                            if (property.Name != "Id")
                            {
                                var newValue = property.GetValue(newProfile);
                                property.SetValue(existingProfile, newValue);
                            }
                        }

                        await db.SaveChangesAsync();
                        updatedProfile = existingProfile; // Возвращаем обновленный профиль
                    }
                    else
                    {
                        // Если профиль не найден, создаем новый
                        Console.WriteLine("Добавляем новый профиль.");
                        db.BitTorrConfig.Add(newProfile);
                        await db.SaveChangesAsync();
                        updatedProfile = newProfile; // Возвращаем новый профиль
                    }
                    return Task.CompletedTask;  
                });
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Ошибка при получении или обновлении профиля: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Stack trace: {ex.InnerException.StackTrace}");
                }
            }

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
        public static async Task<TextInputFlag> GetTextInputFlag()
        {
            return await WithDbContextAsync(async db =>
            {
                var textInputFlags = db.TextInputFlag.FirstOrDefault(x => x.IdChat == adminChat);
               
                return textInputFlags;
            });
        }
        public static async Task<bool> IsTextInputFlagLogin()
        {
            Console.WriteLine("Заход в IsTextInputFlagLogin");

            // Выполняем и получаем результат из WithDbContextAsync
            var result = await WithDbContextAsync(async db =>
            {
                var textInputFlags = await db.TextInputFlag.FirstOrDefaultAsync(x => x.IdChat == adminChat);
                var flagLogin = textInputFlags?.FlagLogin ?? false;

                Console.WriteLine($"flaglogin:{flagLogin}");

                return flagLogin;
            });

            return result;

        }


        public static async Task<bool> SwitchTorSettingsInputFlag(string nameFlag, bool flag)
        {
            return await WithDbContextAsync(async db =>
            {
                try
                {
                    // Получаем объект из базы данных
                    var textInputFlags = await db.TextInputFlag.FirstOrDefaultAsync(x => x.IdChat == adminChat);
                    if (textInputFlags == null)
                    {
                        Console.WriteLine($"Объект с IdChat = {adminChat} не найден.");
                        return false;
                    }

                    // Вывод всех свойств и их текущих значений для отладки
                    Console.WriteLine("Список свойств и их текущих значений:");
                    foreach (var prop in typeof(TextInputFlag).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var value = prop.GetValue(textInputFlags);
                        Console.WriteLine($"[{prop.Name}] = {value}");
                    }

                    // Ищем свойство по имени
                    var property = typeof(TextInputFlag).GetProperty(nameFlag, BindingFlags.Public | BindingFlags.Instance);
                    if (property == null)
                    {
                        Console.WriteLine($"Свойство [{nameFlag}] не найдено в классе TextInputFlag.");
                        return false;
                    }

                    // Проверяем, что свойство имеет тип bool и его можно изменить
                    if (property.CanWrite && property.PropertyType == typeof(bool))
                    {
                        property.SetValue(textInputFlags, flag); // Обновляем значение свойства
                        if (flag)
                        {
                            textInputFlags.LastTextFlagTrue =nameFlag;
                         
                        }
                        
                        Console.WriteLine($"Свойство {nameFlag} успешно обновлено на {flag}.");
                    }
                    else
                    {
                        Console.WriteLine($"Свойство {nameFlag} нельзя изменить или оно не является типом bool.");
                        return false;
                    }

                    // Сохраняем изменения в базе данных
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обновлении свойства {nameFlag}: {ex.Message}");
                    return false;
                }
            });
        }


        public static async Task SwitchTimeZone(string indicator)
        {
            await WithDbContextAsync(async db =>
            {
                var setBot = db.SettingsBot.FirstOrDefault(x => x.IdChat == adminChat);
                setBot.ChangeTimeZone(indicator);
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            });
        }
  
        public static async Task SwitchAutoChangePassTorrserver(bool isActive)
        {
           await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsTorrserverBot.FirstOrDefault(x => x.idChat == adminChat);
                setTorr.IsActiveAutoChange = isActive;
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            });
            
        }
        public static async Task<SettingsBot> GetSettingBot()
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsBot.FirstOrDefault(x => x.IdChat == adminChat);
                return setTorr;
            });

        }
        public static async Task SetLoginPasswordSettingsTorrserverBot(string login,string password)
        {
            await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsTorrserverBot.FirstOrDefault(x => x.idChat == adminChat);
                setTorr.Login = login;
                setTorr.Password = password;
              await  db.SaveChangesAsync();
                return Task.CompletedTask;
            });

        }
        public static async Task<SettingsTorrserverBot> GetSettingsTorrserverBot()
        {
         return   await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = db.SettingsTorrserverBot.FirstOrDefault(x => x.idChat == adminChat);
                return setTorr;
            });
           
        }
        public static async Task CheckAndInsertDefaultData()
        {
            
            await SqlMethods.WithDbContextAsync(async db =>
            {
                // Применяем все необходимые миграции
                await db.Database.MigrateAsync();

                // Убедимся, что таблицы созданы
              //  await db.Database.EnsureCreatedAsync();
                var existingSettingsBot = await db.SettingsBot.FirstOrDefaultAsync(s => s.IdChat == adminChat);
                var existingSettingsTorrserver = await db.SettingsTorrserverBot.FirstOrDefaultAsync(s => s.idChat == adminChat);
                var existingUser = await db.User.FirstOrDefaultAsync(u => u.IdChat == adminChat);
                var existingTextInputFlags = await db.TextInputFlag.FirstOrDefaultAsync(u => u.IdChat == adminChat);
                var existingBitTorrConfig = await db.BitTorrConfig.FirstOrDefaultAsync(u=>u.IdChat== adminChat);
                var entitiesToAdd = new List<object>();
                if (existingTextInputFlags == null)
                {
                    entitiesToAdd.Add(new TextInputFlag { IdChat = adminChat });
                }
                if (existingSettingsBot == null)
                    entitiesToAdd.Add(new SettingsBot { IdChat = adminChat });

                if (existingSettingsTorrserver == null)
                    entitiesToAdd.Add(new SettingsTorrserverBot { idChat = adminChat });

                if (existingUser == null)
                    entitiesToAdd.Add(new Model.User { IdChat = adminChat });
                if (existingBitTorrConfig == null)
                {
                    entitiesToAdd.Add(new BitTorrConfig { IdChat = adminChat });
                }
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
