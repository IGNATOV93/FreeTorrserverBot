using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AdTorrBot.BotTelegram.Db.Model.TorrserverModel
{
    public class Profiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Автоинкремент для Id
        public int Id { get; set; }

        public Guid UniqueId { get; set; } = Guid.NewGuid();

        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? AdminComment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get;set; }

        public DateTime? AccessEndDate { get; set; }

        public bool IsEnabled { get; set; } = true;

    }
}
