using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram.Db.Model
{
    public class SettingsBot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string IdChat { get; set; } = string.Empty;

        public double TimeZoneOffset { get; set; } = 3.0; // UTC+3 для Москвы
      
        // Метод для изменения часового пояса
        public void ChangeTimeZone(string direction)
        {
            if (direction == "+")
            {
                // Увеличиваем смещение на 1
                TimeZoneOffset = Math.Min(TimeZoneOffset + 1, 14.0); // Максимум +14
            }
            else if (direction == "-")
            {
                // Уменьшаем смещение на 1
                TimeZoneOffset = Math.Max(TimeZoneOffset - 1, -12.0); // Минимум -12
            }
        }
    }
}
