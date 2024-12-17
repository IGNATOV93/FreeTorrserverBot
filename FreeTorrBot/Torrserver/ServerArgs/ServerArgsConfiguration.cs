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
        public static ServerArgsConfig ReadConfigArgs(string filePath)
        {
            var configLine = File.ReadAllText(filePath);  // Чтение всего содержимого файла
            return ParseConfigArgs(configLine);  // Парсим строку в объект конфигурации
        }

        // Метод для записи конфигурации в файл
        public static void WriteConfigArgs(string filePath, ServerArgsConfig config)
        {
            var configLine = SerializeConfigArgs(config); // Сериализуем объект конфигурации в строку
            File.WriteAllText(filePath, configLine); // Записываем строку в файл
        }

        // Метод для парсинга строки конфигурации в объект ServerArgsConfig
        public static ServerArgsConfig ParseConfigArgs(string configLine)
        {
            var options = new ServerArgsConfig();  // Новый объект конфигурации

            // Используем регулярное выражение для извлечения строки параметров из DAEMON_OPTIONS
            var match = Regex.Match(configLine, @"DAEMON_OPTIONS\s*=\s*""(.*)""");
            if (!match.Success) return options; // Если не удалось извлечь параметры, возвращаем пустой объект

            var parameters = match.Groups[1].Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);  // Разбиваем строку на параметры

            // Перебор всех параметров
            foreach (var parameter in parameters)
            {
                if (parameter.StartsWith("--"))  // Проверка, что параметр начинается с "--"
                {
                    var parts = parameter.Split(new[] { '=' }, 2);  // Разделяем параметр на ключ и значение
                    var key = parts[0].TrimStart('-');  // Извлекаем ключ
                    var value = parts.Length > 1 ? parts[1] : "true";  // Если значение не указано, присваиваем "true"

                    // Поиск свойства с атрибутом ConfigOption, где ключ совпадает с параметром
                    var property = typeof(ServerArgsConfig).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttribute<ConfigOptionAttribute>()?.Key == key);

                    if (property != null)
                    {
                        var convertedValue = Convert.ChangeType(value, property.PropertyType);  // Преобразуем значение в нужный тип
                        property.SetValue(options, convertedValue);  // Устанавливаем значение свойства объекта конфигурации
                    }
                }
            }

            return options;  // Возвращаем объект конфигурации с установленными значениями
        }
    }

}
