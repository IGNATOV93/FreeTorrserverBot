using System.IO;
using System.Diagnostics;
using FreeTorrserverBot.BotTelegram;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using FreeTorrBot.BotTelegram.BotSettings.Model;
using FreeTorrBot.BotTelegram.BotSettings;
using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using System.Text.Json;
using System;
using System.Reflection;
using System.Text.RegularExpressions;


namespace FreeTorrserverBot.Torrserver
{
    public abstract class Torrserver

    {
        static string  nameProcesTorrserver = "TorrServer-linux-amd64";
        static string filePathTorrMain = TelegramBot.settingsJson.FilePathTorrserver;
        static string filePathTorrserverDb = @$"{filePathTorrMain}accs.db";
        static string filePathTorr = @$"{filePathTorrMain}{nameProcesTorrserver}";
        static string filePathSettingsJson = @$"{filePathTorrMain}settings.json";


        #region MainPforile
        public static async Task AutoChangeAccountTorrserver()
        {
            var settings = await SqlMethods.GetSettingsTorrserverBot();
            if (settings != null&&settings.IsActiveAutoChange==true)
            {
                var inlineKeyboarDeleteMessageOnluOnebutton = new InlineKeyboardMarkup(new[]
                  {new[]{InlineKeyboardButton.WithCallbackData("Скрыть \U0001F5D1", "deletemessages")}});

                await ChangeMainAccountTorrserver("","",false,true);
                await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"Произведена автосмена пароля сервера \U00002705\r\n" +
                                                                                      $"\U0001F570   {DateTime.Now}", replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
                await TelegramBot.client.SendTextMessageAsync(TelegramBot.AdminChat, $"{TakeMainAccountTorrserver()}", replyMarkup: inlineKeyboarDeleteMessageOnluOnebutton);
            }
           
            return;
        }


        //Смена пароля/логина главного аккаунта(профиля torrserver)
        public static async Task ChangeMainAccountTorrserver(string login, string password, bool setLogin, bool setPassword)
        {
            var newParolRandom = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var settingsTorr = await SqlMethods.GetSettingsTorrserverBot();
            string newPassword = string.IsNullOrEmpty(password) ? settingsTorr.Password : password;
            string newLogin = string.IsNullOrEmpty(login) ? settingsTorr.Login : login;

            if (setLogin && string.IsNullOrEmpty(login))
            {
                newLogin = new string(Enumerable.Repeat(chars, 10)
                                  .Select(s => s[newParolRandom.Next(s.Length)]).ToArray());
            }

            if (setPassword && string.IsNullOrEmpty(password))
            {
                newPassword = new string(Enumerable.Repeat(chars, 10)
                                  .Select(s => s[newParolRandom.Next(s.Length)]).ToArray());
            }

            await SqlMethods.SetLoginPasswordSettingsTorrserverBot(newLogin, newPassword);
            string newAccount = $"\"{newLogin}\":\"{newPassword}\"";

            try
            {
                if (File.Exists(filePathTorrserverDb))
                {
                    var lines = File.ReadAllLines(filePathTorrserverDb).ToList();
                    string content = string.Join("", lines).Trim(); // Считываем всё содержимое как одну строку

                    if (content.StartsWith("{") && content.EndsWith("}"))
                    {
                        content = content.Substring(1, content.Length - 2); // Убираем внешние фигурные скобки

                        // Разбиваем на аккаунты по запятой
                        var accounts = content.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (accounts.Count > 0)
                        {
                            // Заменяем первый аккаунт
                            accounts[0] = newAccount;
                        }
                        else
                        {
                            // Если список пуст, добавляем новый аккаунт
                            accounts.Add(newAccount);
                        }

                        // Собираем всё обратно в строку
                        string updatedContent = $"{{{string.Join(",", accounts)}}}";
                        File.WriteAllText(filePathTorrserverDb, updatedContent);
                    }
                    else
                    {
                        // Если файл пуст или формат некорректен, создаем новый файл с одним аккаунтом
                        File.WriteAllText(filePathTorrserverDb, $"{{{newAccount}}}");
                    }
                }
                else
                {
                    // Если файла нет, создаем новый файл с одним аккаунтом
                    File.WriteAllText(filePathTorrserverDb, $"{{{newAccount}}}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении файла: {ex.Message}");
            }

            await RebootingTorrserver();
        }

        public static async Task RebootingTorrserver()
        {
            // Завершаем процесс
            var killProcess = new ProcessStartInfo
            {
                FileName = "killall",
                Arguments = nameProcesTorrserver,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(killProcess))
            {
                await process.WaitForExitAsync(); // Ожидаем завершения процесса killall
            }

            // Запускаем процесс заново
            var startProcess = new ProcessStartInfo
            {
                FileName = $"{filePathTorrMain}{nameProcesTorrserver}", // Укажите полный путь к файлу, если он не в PATH
                UseShellExecute = true,
            };

            Process.Start(startProcess);
        }

        public static string TakeMainAccountTorrserver()
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePathTorrserverDb))
                {
                    // Считываем весь файл как одну строку
                    string content = reader.ReadToEnd().Trim();

                    if (content.StartsWith("{") && content.EndsWith("}"))
                    {
                        // Убираем внешние фигурные скобки
                        content = content.Substring(1, content.Length - 2);

                        // Разбиваем строки по запятой
                        var accounts = content.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        if (accounts.Length > 0)
                        {
                            // Берем первый аккаунт
                            string firstAccount = accounts[0].Trim();

                            // Находим логин и пароль
                            int keyStartIndex = firstAccount.IndexOf("\"") + 1;
                            int keyEndIndex = firstAccount.IndexOf("\":");
                            int valueStartIndex = firstAccount.IndexOf(":\"") + 2;
                            int valueEndIndex = firstAccount.LastIndexOf("\"");

                            if (keyStartIndex >= 0 && keyEndIndex > keyStartIndex &&
                                valueStartIndex > keyEndIndex && valueEndIndex > valueStartIndex)
                            {
                                string login = firstAccount.Substring(keyStartIndex, keyEndIndex - keyStartIndex);
                                string password = firstAccount.Substring(valueStartIndex, valueEndIndex - valueStartIndex);

                                return $"{login}:{password}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }
            return "Ошибка чтения."; // Возвращаем сообщение об ошибке
        }
        #endregion MainProfile
        #region OtherProfiles

        //Обловляем что у нас есть в конфиге пользователи с бд данными .
        public static async Task<bool> UpdateAllProfilesFromConfig()
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePathTorrserverDb))
                {
                    // Считываем весь файл как одну строку
                    string content = reader.ReadToEnd().Trim();

                    if (content.StartsWith("{") && content.EndsWith("}"))
                    {
                        // Убираем внешние фигурные скобки
                        content = content.Substring(1, content.Length - 2);

                        // Разбиваем строки по запятой
                        var accounts = content.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var profiles = new List<Profiles>();
                        var uniqueLogins = new HashSet<string>(); // Для отслеживания уникальных логинов
                        foreach (var account in accounts)
                        {
                            int keyStartIndex = account.IndexOf("\"") + 1;
                            int keyEndIndex = account.IndexOf("\":");
                            int valueStartIndex = account.IndexOf(":\"") + 2;
                            int valueEndIndex = account.LastIndexOf("\"");

                            if (keyStartIndex >= 0 && keyEndIndex > keyStartIndex &&
                                valueStartIndex > keyEndIndex && valueEndIndex > valueStartIndex)
                            {
                                string login = account.Substring(keyStartIndex, keyEndIndex - keyStartIndex);
                                string password = account.Substring(valueStartIndex, valueEndIndex - valueStartIndex);

                                // Проверяем, есть ли уже такой логин
                                if (!uniqueLogins.Contains(login))
                                {
                                    uniqueLogins.Add(login); // Добавляем логин в уникальные
                                    profiles.Add(new Profiles
                                    {
                                        Login = login,
                                        Password = password,
                                        IsEnabled = true,
                                        UpdatedAt = DateTime.UtcNow
                                    });
                                }
                            }
                        }
                        
                        return await SqlMethods.UpdateOrAddProfilesAsync(profiles);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке профилей: {ex.Message}");
            }

            return false;
        }

        #endregion OtherProfiles
    }

}
