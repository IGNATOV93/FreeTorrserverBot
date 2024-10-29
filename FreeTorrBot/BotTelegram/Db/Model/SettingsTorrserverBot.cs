using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore.Query.Internal;
using FreeTorrserverBot.Torrserver;

namespace AdTorrBot.BotTelegram.Db.Model
{
    public  class SettingsTorrserverBot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? idChat { get; set; }

        public string? Login { get; set; }="adTorrBot";
        public string? Password { get; set; }
        public bool IsActiveAutoChange { get; set; }=false;
        public string TimeAutoChangePassword { get; set; } = "20:00";
        // Переопределение метода ToString()
        public override  string ToString()
        {
            var localTime =  Torrserver.GetLocalServerTime();

            // Получаем локальное смещение времени и часовой пояс пользователя
           // var localTimeZone = Torrserver.GetLocalServerTimeTimeZone();
           // var timeZoneUser = SqlMethods.GetSettingBot(idChat).Result.TimeZoneOffset;
            // Форматируем строки для отображения

            //string localTimeZoneString = $"UTC{(localTimeZone >= 0 ? "+" : "")}{localTimeZone}";
           // string timeZoneUserString = $"UTC{(timeZoneUser >= 0 ? "+" : "")}{timeZoneUser}";
            return
               
                $"Статус автосмены: {(IsActiveAutoChange ? "✅" : "❌")}\r\n" +
                $"\u23F0 Автосмена пароля : {TimeAutoChangePassword} "
                //+$"\uD83C\uDF0F {timeZoneUserString}\r\n"
                //+$"\uD83D\uDD50 Время сервера : {localTime} \uD83C\uDF0F {localTimeZoneString}\r\n" 
                ;
        }
    }
}
