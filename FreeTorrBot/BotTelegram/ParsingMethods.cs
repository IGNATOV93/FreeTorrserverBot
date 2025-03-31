using AdTorrBot.BotTelegram.Db.Model.TorrserverModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram
{
    
     public abstract class ParsingMethods
      {
        public static string EscapeForMarkdownV2(string text)
        {
            // Список символов, которые необходимо экранировать в MarkdownV2
            var charactersToEscape = new[] { "_", "*", "[", "]", "(", ")", "~", "`", ">", "#", "+", "-", "=", "|", "{", "}", ".", "!" };

            // Проходимся по каждому символу и заменяем его на экранированный
            foreach (var character in charactersToEscape)
            {
                text = text.Replace(character, $"\\{character}");
            }

            return text;
        }

        public static  string FormatProfilesList(List<Profiles> profiles, int countActive,int countAll,int countSkip, string sort)
        {
            int countInActive = Math.Max(0, countAll - countActive);

            var result = $"📊 Профили: {countAll} (🟢{countActive-1}/🔴{countInActive})\r\n\r\n";
            var countActual = countSkip;
            for (int i = 0; i < profiles.Count; i++)
            {
                countActual++;
                var profile = profiles[i];
                var uni = profile.UniqueId.ToString().Replace("-", "_");
                result += $"\n{countActual}) \uD83D\uDC64 Логин: {profiles[i].Login}\r\n" +
                    $"/showlogpass_{profile.Login}_{profile.Password}\r\n";
                result += $"   {(profile.IsEnabled ? "🟢" : "🔴")} (до {profile.AccessEndDate?.ToString("yyyy-MM-dd") ?? "(не ограничено)"})\r\n";
                result += $"/edit_profile_{uni}\r\n"; //
            }

            result += $"\nСортировка:\n{(sort == "sort_active" ? "🟢" : sort == "sort_inactive" ? "🔴" : "📅")} {sort}\n";
            return EscapeForMarkdownV2(result);
        }

        public static (int count, string sort) ParseOtherProfilesCallback(string callbackData)
        {
            // Проверяем, содержит ли строка "OtherProfiles"
            if (!callbackData.Contains("OtherProfiles"))
            {
                throw new ArgumentException("Данные не соответствуют ожидаемому формату.");
            }

            // Разбиваем строку по "OtherProfiles"
            var parts = callbackData.Split("OtherProfiles");

            // Извлекаем левую часть (count) и правую часть (sort)
            if (parts.Length == 2)
            {
                if (int.TryParse(parts[0], out int count))
                {
                    string sort = parts[1]; // Правая часть как метод сортировки
                    return (count, sort);
                }
            }

            throw new ArgumentException("Невозможно распарсить данные.");
        }

        public static string GetExitMessage(string field)
        {
            // Используем switch или if для определения сообщения в зависимости от поля
            switch (field)
            {
                case "FlagLogin":
                    return "Вы вышли из режима ввода логина Torrserver. ✅";

                case "FlagPassword":
                    return "Вы вышли из режима ввода пароля Torrserver. ✅";

                case "FlagTorrSettCacheSize":
                    return "Вы вышли из режима ввода размера кеша (МБ). ✅";

                case "FlagTorrSettReaderReadAHead":
                    return "Вы вышли из режима ввода значения опережающего кеша (%). ✅";

                case "FlagTorrSettPreloadCache":
                    return "Вы вышли из режима ввода значения буфера предзагрузки (%). ✅";

                case "FlagTorrSettTorrentDisconnectTimeout":
                    return "Вы вышли из режима ввода тайм-аута отключения торрентов (сек). ✅";

                case "FlagTorrSettConnectionsLimit":
                    return "Вы вышли из режима ввода лимита соединений для торрентов. ✅";

                case "FlagTorrSettDownloadRateLimit":
                    return "Вы вышли из режима ввода ограничения скорости загрузки (кб/с). ✅";

                case "FlagTorrSettUploadRateLimit":
                    return "Вы вышли из режима ввода ограничения скорости отдачи (кб/с). ✅";

                case "FlagTorrSettPeersListenPort":
                    return "Вы вышли из режима ввода порта для входящих подключений. ✅";

                case "FlagTorrSettFriendlyName":
                    return "Вы вышли из режима ввода имени сервера DLNA. ✅";

                case "FlagTorrSettRetrackersMode":
                    return "Вы вышли из режима ввода режима ретрекеров. ✅";

                case "FlagTorrSettSslPort":
                    return "Вы вышли из режима ввода SSL порта. ✅";

                case "FlagTorrSettSslCert":
                    return "Вы вышли из режима ввода пути к SSL сертификату. ✅";

                case "FlagTorrSettSslKey":
                    return "Вы вышли из режима ввода пути к SSL ключу. ✅";

                case "FlagTorrSettTorrentsSavePath":
                    return "Вы вышли из режима ввода пути для сохранения торрентов. ✅";



                case "FlagServerArgsSettLogPath":
                    return "Вы вышли из режима ввода пути для логов сервера. ✅";

                case "FlagServerArgsSettPath":
                    return "Вы вышли из режима ввода пути к базе данных и конфигурации. ✅";

                case "FlagServerArgsSettSslPort":
                    return "Вы вышли из режима ввода HTTPS порта. ✅";

                case "FlagServerArgsSettSslCert":
                    return "Вы вышли из режима ввода пути к SSL-сертификату. ✅";

                case "FlagServerArgsSettSslKey":
                    return "Вы вышли из режима ввода пути к SSL-ключу. ✅";

                case "FlagServerArgsSettWebLogPath":
                    return "Вы вышли из режима ввода пути для логов веб-доступа. ✅";

                case "FlagServerArgsSettTorrentsDir":
                    return "Вы вышли из режима ввода директории автозагрузки торрентов. ✅";

                case "FlagServerArgsSettTorrentAddr":
                    return "Вы вышли из режима ввода адреса торрент-клиента. ✅";

                case "FlagServerArgsSettPubIPv4":
                    return "Вы вышли из режима ввода публичного IPv4. ✅";

                case "FlagServerArgsSettPubIPv6":
                    return "Вы вышли из режима ввода публичного IPv6. ✅";

                case "LoginPasswordOtherProfile":
                    return "Вы вышли из режима ввода логина/пароля Torrserver. ✅";
                default:
                    return "Неизвестное поле. ✅";
            }
        }


        public static string UpdateTimeString(string time,int minutesToAdd)
        {
            // Парсим строку "HH:mm" в объект DateTime
            DateTime dateTime = DateTime.ParseExact(time, "HH:mm", null);

            // Добавляем (или вычитаем) заданное количество минут
            dateTime = dateTime.AddMinutes(minutesToAdd);

            // Возвращаем строку с обновленным временем в формате "HH:mm"
            return dateTime.ToString("HH:mm");
        }
        public static int ExtractTimeChangeValue(string valueInput)
        {
            var valueString = valueInput.Split("setAutoPassMinutes")[0].Trim(); // Получаем строку и убираем лишние пробелы
            int value;

            if (int.TryParse(valueString, out value))
            {
                // Успешно преобразовано в int
                Console.WriteLine($"Числовое значение: {value}");
            }
            else
            {
                // Обработка ошибки, если преобразование не удалось
                value = 0;
                Console.WriteLine("Ошибка: значение не может быть преобразовано в число.");
            }
            return value;
        }
      }
}
