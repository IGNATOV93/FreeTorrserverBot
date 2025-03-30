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

        public override string ToString()
        {
            var remainingTime = AccessEndDate.HasValue
                ? AccessEndDate.Value - DateTime.UtcNow
                : (TimeSpan?)null;
            var builder = new StringBuilder();

            builder.AppendLine($"{(IsEnabled ? "🟢" : "🔴")} Профиль \u2199\r\n{UniqueId}\r\n");
            builder.AppendLine($"👤 Логин: {Login}\r\n");
              
            builder.AppendLine($"/showlogpass_{Login}_{Password}\r\n");
            builder.AppendLine($"\r\n📅 Создан: {(CreatedAt.HasValue ? CreatedAt.Value.ToString("dd.MM.yyyy HH:mm") : "Не задано")}");
            builder.AppendLine($"✏️ Редактирован: {(UpdatedAt.HasValue ? UpdatedAt.Value.ToString("dd.MM.yyyy HH:mm") : "Не задано")}");
            builder.AppendLine($"⏳ Окончание доступа: {(AccessEndDate.HasValue ? AccessEndDate.Value.ToString("dd.MM.yyyy HH:mm") : "Не задано")}");
            if (remainingTime.HasValue && remainingTime.Value.TotalMilliseconds > 0)
            {
                builder.AppendLine($"🕒 Осталось: {remainingTime.Value.Days} суток {remainingTime.Value.Hours} часов");
            }
            else
            {
                builder.AppendLine($"🕒 Осталось: Не задано или доступ истёк");
            }
            builder.AppendLine($"💬 Заметка: {(AdminComment ?? "Нет данных")}");

            return builder.ToString();
        }

    }
}
