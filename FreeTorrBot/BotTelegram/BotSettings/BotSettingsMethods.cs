using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeTorrBot.BotTelegram.BotSettings.Model;
using FreeTorrserverBot.BotTelegram;

namespace FreeTorrBot.BotTelegram.BotSettings
{
    public  abstract class BotSettingsMethods
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public  enum SettingsField
        {
            YourBotTelegramToken,
            AdminChatId,
            TimeAutoChangePassword,
            FilePathTorrserverBd,
            FilePathTor,
            IsActiveAutoChange,
            LoginDefaultTorrserver
        }
       public static void ConfigureBot()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Настройки бота не заполнены.");
                Console.ResetColor();

                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1️⃣ Заполнить настройки через консоль");
                Console.WriteLine("2️⃣ Открыть settings.json и перезапустить бота");
                Console.WriteLine("3️⃣ Выйти из режима запуска");

                Console.Write("Введите номер действия: ");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        while (true) // Повторный ввод при ошибке
                        {
                            Console.Write("Введите Telegram токен: ");
                            string token = Console.ReadLine();

                            Console.Write("Введите AdminChatId: ");
                            string adminChatId = Console.ReadLine();

                            Console.WriteLine("\n🔍 Проверяем введенные данные...");
                            Console.WriteLine($"➡ Telegram Token: {token}");
                            Console.WriteLine($"➡ Admin Chat ID: {adminChatId}");
                            Console.WriteLine($"➡ File Path Torrserver: /opt/torrserver/ (Можно поменять в settings.json)");

                            Console.Write("\n✅ Данные верны? (Yes/No/Back): ");
                            string confirmation = Console.ReadLine()?.ToLower();

                            if (confirmation == "yes")
                            {
                                var newSettings = new BotSettingsJson
                                {
                                    YourBotTelegramToken = token,
                                    AdminChatId = adminChatId,
                                    FilePathTorrserver = "/opt/torrserver/"
                                };

                                BotSettingsMethods.SaveSettings(newSettings);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("✅ Настройки сохранены! Перезапустите бота.");
                                Console.ResetColor();

                                return; // 🚀 Выход из функции, завершение настройки
                            }
                            else if (confirmation == "back")
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("🔄 Возвращаемся назад в меню...");
                                Console.ResetColor();
                                break; // 🔄 Выход из внутреннего цикла, возврат в меню
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("🔄 Попробуем снова.");
                                Console.ResetColor();
                            }
                        }
                        break;

                    case "2":
                        Console.WriteLine("⚠️ Откройте settings.json, заполните его вручную и перезапустите бота.");
                        return; // 🚪 Выход из функции

                    case "3":
                        Console.WriteLine("🚪 Выход из режима запуска...");
                        return; // ❌ Закрытие режима настройки

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        public static bool ValidateOrCreateSettings()
        {
            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️ Файл настроек не найден. Создаю новый settings.json...");

                var defaultSettings = new BotSettingsJson
                {
                    YourBotTelegramToken = "",
                    AdminChatId = "",
                    FilePathTorrserver = "/opt/torrserver/"
                };

                SaveSettings(defaultSettings);

                Console.WriteLine("⚠️ settings.json создан! Заполните его вручную или введите данные в консоли.");
                Console.ResetColor();
                return false;
            }

            var settings = LoadSettings();

            if (!settings.Validate(out List<string> missingFields))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Настройки бота не заполнены.");
                Console.WriteLine($"⚠️ Заполните следующие параметры: {string.Join(", ", missingFields)}");
                Console.WriteLine("🔄 Вы можете заполнить вручную в settings.json или прямо сейчас в консоли.");
                Console.WriteLine("1️⃣ Заполнить настройки прямо сейчас");
                Console.WriteLine("2️⃣ Открыть settings.json и перезапустить бота");
                Console.WriteLine("3️⃣ Выйти из настройки");
                Console.ResetColor();

                Console.Write("Введите номер действия: ");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        ConfigureBot(); // Запускаем процесс настройки в консоли
                        return ValidateOrCreateSettings(); // Повторно проверяем настройки после ввода

                    case "2":
                        Console.WriteLine("⚠️ Откройте settings.json, заполните его и попробуйте снова.");
                        return false;

                    case "3":
                        Console.WriteLine("🚪 Выход из режима настройки...");
                        return false;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        Console.ResetColor();
                        return false;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Настройки бота корректны.");
            Console.ResetColor();
            return true;
        }

        public static void UpdateSettings(SettingsField field, string newValue)
        {
            var settings = LoadSettings();

            if (settings == null) return;

            switch (field)
            {
                case SettingsField.YourBotTelegramToken:
                    settings.YourBotTelegramToken = newValue;
                    break;
                case SettingsField.AdminChatId:
                    settings.AdminChatId = newValue;
                    break;
                case SettingsField.FilePathTorrserverBd:
                    settings.FilePathTorrserver = newValue;
                    break;
               
            }

            SaveSettings(settings);
            TelegramBot.settingsJson = LoadSettings();
            Console.WriteLine($"{field} успешно обновлено.");
        }
        public static BotSettingsJson LoadSettings()
        {
            var json = File.ReadAllText(path);
            var settings = JsonConvert.DeserializeObject<BotSettingsJson>(json);

            // Проверка, что настройки загружены
            if (settings == null)
            {
                throw new InvalidOperationException("Не удалось загрузить настройки: настройки равны null.");
            }


            // Возвращаем валидные настройки
            return settings;
        }
        public static void SaveSettings(BotSettingsJson settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
