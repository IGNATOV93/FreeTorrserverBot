using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram
{
    
     public abstract class ParsingMethods
      {

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
