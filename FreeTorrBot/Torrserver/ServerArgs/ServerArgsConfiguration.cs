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
            var parameters = new List<string>();  // Список для хранения аргументов

            // Перебор всех свойств объекта конфигурации ServerArgsConfig
            foreach (var property in typeof(ServerArgsConfig).GetProperties())
            {
                // Проверка наличия атрибута ConfigOption у свойства
                var attribute = property.GetCustomAttribute<ConfigOptionAttribute>();
                if (attribute == null) continue;  // Если атрибут отсутствует, пропускаем свойство

                var value = property.GetValue(config); // Получаем значение свойства
                if (value == null) continue; // Если значение свойства null, пропускаем его

                // Форматируем значение: если это bool и значение true, то не добавляем "=true"
                var formattedValue = property.PropertyType == typeof(bool) && (bool)value ? "" : $"={value}";

                // Добавляем аргумент с ключом и значением
                parameters.Add($"--{attribute.Key}{formattedValue}");
            }

            // Возвращаем строку с аргументами, заключёнными в DAEMON_OPTIONS
            return $"DAEMON_OPTIONS=\"{string.Join(" ", parameters)}\"";
        }

        // Метод для чтения конфигурации из файла
        public static ServerArgsConfig ReadConfigArgs()
        {
            // Проверка, существует ли файл
            if (!File.Exists(filePathTorrserverConfig))
            {
                Console.WriteLine("Конфиг args torrserver не найден,делаем по дефолту его .");
                // Если файл не существует, создаем его и записываем начальную конфигурацию
                var defaultConfig = new ServerArgsConfig(); // Предположим, что у вас есть класс по умолчанию для конфигурации
                WriteConfigArgs(defaultConfig); // Используем ваш метод для записи конфигурации
            }

            // Чтение всего содержимого файла
            var configLine = File.ReadAllText(filePathTorrserverConfig);
            Console.WriteLine(configLine);
            return ParseConfigArgs(configLine); // Парсим строку в объект конфигурации
        }


        // Метод для записи конфигурации в файл
        public static void WriteConfigArgs (ServerArgsConfig config)
        {
            var configLine = SerializeConfigArgs(config); // Сериализуем объект конфигурации в строку
            File.WriteAllText(filePathTorrserverConfig, configLine); // Записываем строку в файл
        }

        // Метод для парсинга строки конфигурации в объект ServerArgsConfig

        public static ServerArgsConfig ParseConfigArgs(string configLine)
        {
            var config = new ServerArgsConfig();

            // Ищем строку DAEMON_OPTIONS
            var match = Regex.Match(configLine, @"DAEMON_OPTIONS\s*=\s*""([^""]*)""");
            if (!match.Success)
            {
                Console.WriteLine("Конфигурация не найдена или строка некорректна.");
                return config;
            }

            var args = match.Groups[1].Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var arg in args)
            {
                // Проверяем флаги вида "--key=value"
                if (arg.StartsWith("--"))
                {
                    var parts = arg.Substring(2).Split(new[] { '=' }, 2);
                    var key = parts[0].ToLower();
                    var value = parts.Length > 1 ? parts[1] : "true";

                    // Найти свойство с соответствующим ключом
                    var property = typeof(ServerArgsConfig).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttribute<ConfigOptionAttribute>()?.Key == key);

                    if (property != null)
                    {
                        try
                        {
                            // Преобразуем значение в нужный тип
                            object convertedValue = property.PropertyType switch
                            {
                                Type t when t == typeof(int?) => int.TryParse(value, out int intValue) ? intValue : (int?)null,
                                Type t when t == typeof(bool) => value.ToLower() == "true",
                                _ => value
                            };

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






    }

}
