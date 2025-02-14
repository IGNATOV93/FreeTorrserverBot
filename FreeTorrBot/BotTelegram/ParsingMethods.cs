using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram
{
    
     public abstract class ParsingMethods
      {

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
