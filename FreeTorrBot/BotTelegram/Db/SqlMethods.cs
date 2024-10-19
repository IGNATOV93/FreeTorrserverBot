using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram.Db
{
   public class SqlMethods
    {


        public static async Task ListTablesAsync()
        {
            await SqlMethods.WithDbContextAsync(async db =>
            {
                var entityTypes = db.Model.GetEntityTypes();
                foreach (var entityType in entityTypes)
                {
                    Console.WriteLine(entityType.ClrType.Name); // Имя таблицы
                }
                return Task.CompletedTask; // Возвращаем завершённую задачу
            });
        }

        public static async Task<TResult> WithDbContextAsync<TResult>(Func<AppDbContext,Task<TResult>> func)
        {
            await using var db = new AppDbContext();
            return await func(db);
        }
    }
}
