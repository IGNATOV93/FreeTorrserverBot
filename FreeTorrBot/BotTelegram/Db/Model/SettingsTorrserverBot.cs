using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram.Db.Model
{
    public  class SettingsTorrserverBot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? IdChat { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public bool IsActiveAutoChange { get; set; }=false;
        public string TimeAutoChangePassword { get; set; } = "20:00";
        public string LoginDefaultTorrserver { get; set; } = "adTorrBot";
    }
}
