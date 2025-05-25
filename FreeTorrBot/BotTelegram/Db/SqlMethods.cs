using AdTorrBot.BotTelegram.Db.Model;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using FreeTorrserverBot.BotTelegram;
using FreeTorrserverBot.Torrserver;
using FreeTorrserverBot.Torrserver.BitTor;
using FreeTorrserverBot.Torrserver.ServerArgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
using System.Xml;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdTorrBot.BotTelegram.Db
{
    public abstract class SqlMethods
    {

        public static readonly string adminChat = TelegramBot.AdminChat;

        #region mainProfile
        public static async Task SetSettingsServerArgsProfile(ServerArgsConfig config)
        //SetArgsConfigTorrProfile(ServerArgsConfig config)
        {
            Console.WriteLine($"Запуск SetArgsConfigTorrProfile.");
            try
            {
                // Используем метод для работы с базой данных
                await SqlMethods.WithDbContextAsync(async db =>
                {
                    // Проверяем, существует ли профиль с таким же IdChat и NameProfileBot
                    var existingProfile = await db.ServerArgsConfig
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
                        var profileWithSameIdChat = await db.ServerArgsConfig
                                                             .FirstOrDefaultAsync(x => x.IdChat == adminChat);

                        if (profileWithSameIdChat != null)
                        {
                            Console.WriteLine($"Профиль adminChat = {adminChat} уже существует, но с другим NameProfileBot.");
                            // Если профиль существует с таким IdChat, но с другим NameProfileBot.

                        }

                        // Если профиль с таким IdChat и NameProfileBot не найден, создаем новый
                        Console.WriteLine($"Профиль adminChat = {adminChat} и NameProfileBot = {profileWithSameIdChat?.NameProfileBot} не найден, создаем новый.");
                        config.IdChat = adminChat; // Присваиваем idChat для нового профиля
                        db.ServerArgsConfig.Add(config);
                        await db.SaveChangesAsync();
                        Console.WriteLine("Новый профиль args добавлен в базу данных.");
                    }
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Ошибка при сохранении профиля args в базу данных: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Stack trace: {ex.InnerException.StackTrace}");
                }
            }
        }

        public static async Task<ServerArgsConfig> GetArgsConfigTorrProfile(string idChat)
        {
            Console.WriteLine($"Запуск GetArgsConfigTorrProfile.");
            ServerArgsConfig updatedProfile = null;

            try
            {
                await SqlMethods.WithDbContextAsync(async db =>
                {
                    // Пытаемся найти профиль по IdChat
                    var existingProfile = await db.ServerArgsConfig.FirstOrDefaultAsync(x => x.IdChat == idChat);

                    if (existingProfile != null)
                    {
                        Console.WriteLine($"Профиль  найден adminChat = {idChat}");
                    }
                    else
                    {
                        Console.WriteLine($"Профиль adminChat = {idChat} не найден.");
                    }

                    // Логируем чтение нового профиля
                    ServerArgsConfig newProfile =await ServerArgsConfiguration.ReadConfigArgs(); 
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
                        Console.WriteLine("Обновляем профиль (args).");
                        foreach (var property in typeof(ServerArgsConfig).GetProperties())
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
                        db.ServerArgsConfig.Add(newProfile);
                        await db.SaveChangesAsync();
                        updatedProfile = newProfile; // Возвращаем новый профиль
                    }
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Ошибка при получении или обновлении профиля args: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Stack trace: {ex.InnerException.StackTrace}");
                }
            }

            return updatedProfile;
        }







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
                    BitTorrConfig newProfile = await BitTorrConfigation.ReadConfig();
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

        public static async Task<bool> SwitchOffInputFlag()
        {
            return await WithDbContextAsync(async db =>
            {
                try
                {
                    // Получаем объект из базы данных
                    var textInputFlags = await db.TextInputFlag.FirstOrDefaultAsync(x => x.IdChat == adminChat);
                    if (textInputFlags == null)
                    {
                        Console.WriteLine($"Объект textInputFlags с IdChat = {adminChat} не найден.");
                        return false;
                       
                    }
                    else
                    {
                        var properties = typeof(TextInputFlag).GetProperties()
                     .Where(p => p.PropertyType == typeof(bool));

                        foreach (var prop in properties)
                        {
                            prop.SetValue(textInputFlags, false);
                        }
                        db.TextInputFlag.Update(textInputFlags);
                        await db.SaveChangesAsync();
                        return true;
                    }
                    
                }
                catch (Exception e)
                {
                   return false;
                }
               
            });
          
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
        public static async Task<bool> UpdateSettingsTorrserverBot(SettingsTorrserverBot settings)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr = await db.SettingsTorrserverBot.FirstOrDefaultAsync(x => x.idChat == settings.idChat);
                if (setTorr != null)
                {
                    setTorr.Login = settings.Login;
                    setTorr.Password = settings.Password;
                    setTorr.IsActiveAutoChange = settings.IsActiveAutoChange;
                    setTorr.TimeAutoChangePassword = settings.TimeAutoChangePassword;
                    setTorr.IsTorrserverAutoRestart = settings.IsTorrserverAutoRestart;
                    setTorr.TorrserverRestartTime = settings.TorrserverRestartTime;

                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            });
        }


        public static async Task<SettingsTorrserverBot> GetSettingsTorrserverBot()
        {
         return   await SqlMethods.WithDbContextAsync(async db =>
            {
                var setTorr =await db.SettingsTorrserverBot.FirstOrDefaultAsync(x => x.idChat == adminChat);
             
              if (string.IsNullOrWhiteSpace(setTorr?.TorrserverRestartTime)) // Проверка на null и пустую строку
                {
                    setTorr.TorrserverRestartTime = "20:00";
                    await db.SaveChangesAsync();
                }

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
        #endregion mainProfile
        public async Task<bool> IsHaveLogin(string login)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var profile = await db.Profiles.FirstOrDefaultAsync(predicate => predicate.Login == login);
                return profile != null; // Возвращаем true, если запись существует
            });
        }

        #region OtherPfofiles
        public static async Task<string> GetLastChangeUid()
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var s = await db.SettingsBot.FirstOrDefaultAsync(x => x.IdChat == adminChat);
                if (s is null)
                {
                    return "";
                }
                else
                {
                    return s.LastChangeUid ?? "";
                }
               
            });
        }
        public static async Task<bool> SetLastChangeUid(string uid)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var s = await db.SettingsBot.FirstOrDefaultAsync(x => x.IdChat == adminChat);
                if(s is null)
                {
                    return false;
                }
                else
                {
                    s.LastChangeUid = uid;
                    await db.SaveChangesAsync();
                }
                return true;
            });
        }
        public static async Task<bool> DeleteProfileOther(string uid)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var p = await db.Profiles.FirstOrDefaultAsync(p => p.UniqueId == Guid.Parse(uid));
                if (p != null) {
                    await Torrserver.DeleteProfileByLogin(p.Login);
                    db.Remove(p);
                }
              await  db.SaveChangesAsync();
               
                return true;
            });
        }
        public static async Task<int> GetActiveProfilesCount()
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
               await SqlMethods.UpdateIsActiveProfiles();
                // Обрабатываем записи, где IsEnabled явно true
                var activeCount = await db.Profiles.CountAsync(p => p.IsEnabled == true)-1;
                Console.WriteLine($"Активных пользователей: "+activeCount);
                return (activeCount<0? 0:activeCount);
            });
        }


        public static async Task<int> GetCountAllProfiles()
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                await SqlMethods.UpdateIsActiveProfiles();
                int count = await db.Profiles.CountAsync()-1;
                return Math.Max(count, 0); 
            });
        }

        //Берем пользователей
        public static async Task<bool> UpdateIsActiveProfiles()
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var profiles = await db.Profiles.ToListAsync();

                foreach (var profile in profiles)
                {
                    profile.IsEnabled = profile.AccessEndDate == null || profile.AccessEndDate > DateTime.Now;
                }

                await db.SaveChangesAsync();
                return true;
            });
        }
        public static async Task<List<Profiles>> GetAllProfilesNoSkip()
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                return await db.Profiles.ToListAsync();
            });
        }
        public static async Task<List<Profiles>> GetAllProfilesUser(int skipCount, string sort)
        {
            var MainLogin = Torrserver.TakeMainAccountTorrserver();
            if (MainLogin != null)
            {
                MainLogin = Torrserver.ParseMainLoginFromTorrserverProfile(MainLogin);
            }
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                IQueryable<Profiles> query = db.Profiles ;

                // Обработка вида сортировки
                if(MainLogin != null)
                {
                  query= query.Where(x=>x.Login != MainLogin) ;
                }
                if (sort == "sort_active")
                {
                    // Сначала пользователи с IsEnabled == true
                    query = query.OrderByDescending(p => p.IsEnabled);
                }
                else if (sort == "sort_inactive")
                {
                    // Сначала пользователи с IsEnabled == false
                    query = query.OrderBy(p => p.IsEnabled);
                }
                else if (sort == "sort_date")
                {
                    // Сортировка по дате окончания времени профиля (AccessEndDate, сначала старые записи)
                    query = query.OrderBy(p => p.AccessEndDate);
                }
                else
                {
                    throw new ArgumentException($"Неизвестный параметр сортировки: {sort}");
                }

                // Пропускаем записи (пагинация) и возвращаем результат
                return await query.Skip(skipCount).Take(5).ToListAsync();
            });
        }

        //Берем пользователя
        public static async Task<Profiles?> GetProfileUser(string? login = null, string? uniqueId = null)
        {
            if (string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(uniqueId))
            {
                throw new ArgumentException("Необходимо указать хотя бы один параметр: login или uniqueId.");
            }

            return await SqlMethods.WithDbContextAsync(async db =>
            {
                Profiles? profile = null;

                if (!string.IsNullOrEmpty(uniqueId))
                {
                    // Поиск по uniqueId
                    profile = await db.Profiles.FirstOrDefaultAsync(p => p.UniqueId.ToString().ToUpper() == uniqueId.ToUpper());
                }
                else if (!string.IsNullOrEmpty(login))
                {
                    // Поиск по логину
                    profile = await db.Profiles.FirstOrDefaultAsync(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
                }

                if (profile == null)
                {
                    Console.WriteLine("Профиль не найден");
                }
                else
                {
                    Console.WriteLine($"Профиль найден: {profile}");
                }

                return profile;
            });
        }
        public static async Task<Profiles?> FindProfileToLoginAndPassword(string log,string pass)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var profile = await db.Profiles.FirstOrDefaultAsync(x => x.Login == log && x.Password == pass);
                if(profile == null) { return null; }
                else
                {
                    return profile;
                }
            });
        }
        public static async Task<bool> AddOtherProfileTorrserve(Profiles newProfile)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                await db.Profiles.AddAsync(newProfile);
                await db.SaveChangesAsync();
                return true;
            });
        }
        //Редактирование профиля пользователя
        public static async Task<bool> EddingProfileUser(Profiles profile)
        {
            Console.WriteLine($"Пришел другой профиль на редактирование: \r\n{profile.ToString()}");
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                var profileUser = await db.Profiles.FirstOrDefaultAsync(p => p.UniqueId == profile.UniqueId);
                if (profileUser != null)
                {
                    // Обновляем только нужные свойства
                    profileUser.Login = profile.Login;
                    profileUser.Password = profile.Password;
                    profileUser.UpdatedAt = DateTime.UtcNow; // Обновляем дату изменения
                    profileUser.AccessEndDate = profile.AccessEndDate;
                    profileUser.IsEnabled = profile.IsEnabled;
                    profileUser.AdminComment = profile.AdminComment;
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            });
        }

        //Создание нового профиля(пользователя) Torrserver
        public static async Task<Profiles?> CreateAuthoNewProfileOther()
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                int attempts = 0;
                const int maxAttempts = 150; // Ограничение на количество попыток
                while (attempts < maxAttempts)
                {
                    var newlogin = InputTextValidator.CreateAutoNewLoginOrPassword();

                    // Проверяем, есть ли такой логин в базе
                    bool loginExists = await db.Profiles.AnyAsync(p => p.Login == newlogin);
                    if (!loginExists)
                    {
                        var password = InputTextValidator.CreateAutoNewLoginOrPassword();
                        var newProfile = new Profiles()
                        {
                            Login = newlogin,
                            Password = password,
                            AccessEndDate = DateTime.UtcNow.AddDays(1),
                            IsEnabled = true
                        };
                        await db.Profiles.AddAsync(newProfile);
                        await db.SaveChangesAsync();
                        return newProfile;
                    }

                    attempts++; // Увеличиваем счётчик попыток
                }

                // Если не удалось создать профиль, просто возвращаем null
                return null;
            });
        }
        //Проверка на существование логина в бд (любого)
        public static async Task<bool> IsLoginExistsAsync(string login)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                return await db.Profiles.AnyAsync(p => p.Login == login);
            });
        }

        //Проверка на существование логина в бд
        public static async Task<bool> IsHaveLoginProfileUser(string login, bool isOther)
        {
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                Console.WriteLine($"В IsHaveLoginProfileUser пришло\r\n" +
                    $"Логин: [{login}]\r\n" +
                    $"Bool: [{isOther}]");
                if (isOther)
                {
                    // Получаем уникальный идентификатор текущего пользователя
                    var settings = await db.SettingsBot.FirstOrDefaultAsync(x=>x.IdChat==adminChat);
                    var actFlagUidUser = settings?.LastChangeUid;
                    Console.WriteLine($"Крайний uid:\r\n" +
                        $"[{actFlagUidUser}]");
                    // Если идентификатор текущего пользователя недоступен, возвращаем false
                    if (string.IsNullOrEmpty(actFlagUidUser))
                    {
                        return false;
                    }

                    // Проверяем, существует ли логин у других пользователей
                    bool loginExistsForOthers = await db.Profiles
                        .AnyAsync(p => p.Login == login && p.UniqueId != Guid.Parse(actFlagUidUser));
                    {
                        // Запрашиваем список профилей с совпадающим логином, но другим UniqueId
                        var conflictingProfiles = await db.Profiles
                            .Where(p => p.Login == login && p.UniqueId != Guid.Parse(actFlagUidUser))
                            .ToListAsync();

                        // Выводим их в консоль
                        foreach (var profile in conflictingProfiles)
                        {
                            Console.WriteLine($"Conflicting Profile: Login = {profile.Login}, UniqueId = {profile.UniqueId}");
                        }

                        // Если список пуст, это значит, что нет конфликтов
                        if (!conflictingProfiles.Any())
                        {
                            Console.WriteLine("Нет профилей с совпадающим логином у других пользователей.");
                        }
                        else
                        {
                            Console.WriteLine($"Найдено {conflictingProfiles.Count} конфликтующих профилей.");
                        }

                    }
                    // Если логин занят другим пользователем, возвращаем false
                    if (loginExistsForOthers)
                    {
                        Console.WriteLine($"Логин: [{login}] занят другим пользователем.\r\n{false}");
                        return false;
                    }

                    // Проверяем, принадлежит ли логин текущему пользователю
                    return await db.Profiles.AnyAsync(p => p.Login == login && p.UniqueId == Guid.Parse(actFlagUidUser));
                }
                else
                {
                    // Проверяем наличие логина без учёта UniqueId
                    var result = await db.Profiles.AnyAsync(p => p.Login == login);
                    Console.WriteLine($"resultHave: {result}");
                    return !result;
                }
            });
        }


        //Просто создаем новые данные профиля в бд,а после уже работаем с ним 
        public static async Task CreateNewProfileUser(string login,string password)
        {
            await SqlMethods.WithDbContextAsync(async db =>
            {
                var profiles = db.Profiles.Add(new Profiles()
                {
                    Login = login,
                    Password = password,

                });
                await db.SaveChangesAsync();
                return Task.CompletedTask;
            });
        }
        //Обновляем профили в бд,например когда нам надо сверить данные с конфига
        public static async Task<bool> UpdateOrAddProfilesAsync(List<Profiles> profiles)
        {
            
            return await SqlMethods.WithDbContextAsync(async db =>
            {
                // Логины из актуального списка
                var incomingLogins = profiles.Select(p => p.Login).ToList();

                // Обрабатываем переданные профили
                foreach (var profile in profiles)
                {
                    var existingProfile = await db.Profiles.FirstOrDefaultAsync(p => p.Login == profile.Login);

                    if (existingProfile != null)
                    {
                        
                        bool isUpdated = false;

                        // Проверяем и обновляем только изменившиеся поля
                        if (existingProfile.Password != profile.Password)
                        {
                            existingProfile.Password = profile.Password;
                            isUpdated = true;
                        }

                        if (existingProfile.AccessEndDate != profile.AccessEndDate)
                        {
                            existingProfile.AccessEndDate = profile.AccessEndDate;
                            isUpdated = true;
                        }

                        if (existingProfile.IsEnabled != profile.IsEnabled)
                        {
                            existingProfile.IsEnabled = profile.IsEnabled;
                            isUpdated = true;
                        }

                        if (existingProfile.AdminComment != profile.AdminComment)
                        {
                            existingProfile.AdminComment = profile.AdminComment;
                            isUpdated = true;
                        }

                        // Обновляем только дату изменения, если были изменения в других полях
                        if (isUpdated)
                        {
                            existingProfile.UpdatedAt = DateTime.UtcNow;
                        }
                    }

                    else
                    {
                        // Добавляем нового пользователя
                        profile.CreatedAt = DateTime.UtcNow; // Устанавливаем дату создания
                        await db.Profiles.AddAsync(profile);
                    }
                }

                var profilesToDisable = await db.Profiles
                     .Where(p => !incomingLogins.Contains(p.Login)) // Проверяем всех, вне зависимости от статуса
                     .ToListAsync();


                // Помечаем их как отключённых
                foreach (var profile in profilesToDisable)
                {
                    profile.IsEnabled = false; // Помечаем как отключёнprofile.AccessEndDate = DateTime.UtcNow;
                    profile.UpdatedAt = DateTime.UtcNow; // Обновляем дату изменения
                }


                // Сохраняем изменения
                await db.SaveChangesAsync();
                return true;
            });
        }



        #endregion OtherProfile

    }
}
