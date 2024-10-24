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
        public override string ToString()
        {
            var localTime =  Torrserver.GetLocalServerTime();
            return
               
                $"Статус автосмены: {(IsActiveAutoChange ? "✅" : "❌")}\r\n" +
                $"\u23F0 Время автосмены пароля: {TimeAutoChangePassword}\r\n" +
                $"\uD83D\uDD50 Время сервера : {localTime}";
        }
    }
}
