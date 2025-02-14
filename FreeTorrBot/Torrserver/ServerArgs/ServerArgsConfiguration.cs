using AdTorrBot.BotTelegram.Db;
using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using FreeTorrserverBot.BotTelegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FreeTorrserverBot.Torrserver.ServerArgs
{
    public class ServerArgsConfiguration
    {
        // Путь к основным файлам конфигурации Torrserver
        static string filePathTorrMain = TelegramBot.settingsJson.FilePathTorrserver;
        static string filePathTorrserverConfig = @$"{filePathTorrMain}torrserver.config"; // Формируется полный путь к файлу конфигурации torrserver.config

        // Метод для сериализации объекта конфигурации в строку командных аргументов
        public static string SerializeConfigArgs(ServerArgsConfig config)
        {
            var parameters = new List<string>(); // Список для хранения аргументов

            // Перебор всех свойств объекта конфигурации ServerArgsConfig
            foreach (var property in typeof(ServerArgsConfig).GetProperties())
            {
                // Проверка наличия атрибута ConfigOption у свойства
                var attribute = property.GetCustomAttribute<ConfigOptionAttribute>();
                if (attribute == null) continue; // Если атрибут отсутствует, пропускаем свойство

                var value = property.GetValue(config); // Получаем значение свойства
                if (value == null) continue; // Если значение свойства null, пропускаем его

                // Если свойство булевое
                if (property.PropertyType == typeof(bool))
                {
                    if ((bool)value) // Если значение true, добавляем только ключ
                    {
                        parameters.Add($"--{attribute.Key}");
                    }
                }
                else
                {
                    // Для всех остальных типов добавляем ключ и значение через пробел
                    parameters.Add($"--{attribute.Key} {value}");
                }
            }

            // Возвращаем строку с аргументами, заключёнными в DAEMON_OPTIONS
            return $"DAEMON_OPTIONS=\"{string.Join(" ", parameters)}\"";
        }



        // Метод для чтения конфигурации из файла
        public static async Task<ServerArgsConfig>  ReadConfigArgs()
        {
            try
            {
                // Проверка, существует ли файл
                if (!File.Exists(filePathTorrserverConfig))
                {
                    Console.WriteLine("Конфигурация (torrserver.config) не найдена. Создаём конфигурацию по умолчанию.");
                    var defaultConfig = new ServerArgsConfig(); // Конфигурация по умолчанию
                 await   WriteConfigArgs(defaultConfig); // Записываем её в файл
                   
                    return defaultConfig;
                }

                // Чтение всего содержимого файла
                var configLine = File.ReadAllText(filePathTorrserverConfig);
                if (string.IsNullOrWhiteSpace(configLine))
                {
                    Console.WriteLine("Конфигурация (torrserver.config) пуста. Используем конфигурацию по умолчанию.");
                    var defaultConfig = new ServerArgsConfig();
                   await WriteConfigArgs(defaultConfig);
                    
                    return defaultConfig;
                }
                 var conf = ParseConfigArgs(configLine);
                  await SqlMethods.SetArgsConfigTorrProfile(conf);
                // Парсим строку в объект конфигурации
                return conf;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении конфигурации(torrserver.config): {ex.Message}");
                throw; // Рекомендуется либо обработать, либо пробросить исключение
            }
        }

        // Метод для записи конфигурации в файл
        public static async Task WriteConfigArgs(ServerArgsConfig config)
        {
            try
            {
                var configLine = SerializeConfigArgs(config); // Сериализуем объект конфигурации в строку

                // Создаём резервную копию существующего файла, если он есть
                if (File.Exists(filePathTorrserverConfig))
                {
                    var backupPath = $"{filePathTorrserverConfig}.bak";
                    File.Copy(filePathTorrserverConfig, backupPath, overwrite: true);
                    Console.WriteLine($"Резервная копия создана (torrserver.config.bak): {backupPath}");
                }

                // Записываем строку в файл
                File.WriteAllText(filePathTorrserverConfig, configLine);
                Console.WriteLine("Конфигурация(torrserver.config) успешно записана.");
                await SqlMethods.SetArgsConfigTorrProfile(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи конфигурации(torrserver.config): {ex.Message}");
                throw; // Рекомендуется либо обработать, либо пробросить исключение
            }
        }


        // Метод для парсинга строки конфигурации в объект ServerArgsConfig

        public static ServerArgsConfig ParseConfigArgs(string configLine)
        {
            var config = new ServerArgsConfig();

            // Ищем строку DAEMON_OPTIONS
            var match = Regex.Match(configLine, @"DAEMON_OPTIONS\s*=\s*""([^""]*)""");
            if (!match.Success)
            {
                Console.WriteLine("Конфигурация(torrserver.config) не найдена или строка некорректна.");
                return config;
            }

            var args = match.Groups[1].Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                // Если аргумент начинается с "--"
                if (arg.StartsWith("--"))
                {
                    var key = arg.Substring(2).ToLower(); // Убираем "--"

                    // Проверяем следующий аргумент
                    string? value = null;
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
                    {
                        // Следующий аргумент — это значение
                        value = args[i + 1];
                        i++; // Пропускаем следующий аргумент, так как он уже обработан
                    }
                    else
                    {
                        // Если следующего значения нет, значит это булевое значение
                        value = "true";
                    }

                    // Найти свойство с соответствующим ключом
                    var property = typeof(ServerArgsConfig).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttribute<ConfigOptionAttribute>()?.Key == key);

                    if (property != null)
                    {
                        try
                        {
                            // Преобразуем значение в нужный тип
                            var convertedValue = ConvertValue(value, property.PropertyType);
                            property.SetValue(config, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка парсинга ключа {key}: {ex.Message}");
                        }
                    }
                }
            }

            return config;
        }



        /// <summary>
        /// Преобразует строковое значение в указанный тип.
        /// </summary>
        private static object ConvertValue(string value, Type targetType)
        {
            return targetType switch
            {
                Type t when t == typeof(int?) => int.TryParse(value, out int intValue) ? intValue : null,
                Type t when t == typeof(long?) => long.TryParse(value, out long longValue) ? longValue : null,
                Type t when t == typeof(bool) => value.ToLower() == "true",
                Type t when t == typeof(string) => value,
                _ => throw new InvalidOperationException($"Неподдерживаемый тип: {targetType}")
            };
        }







    }

}
