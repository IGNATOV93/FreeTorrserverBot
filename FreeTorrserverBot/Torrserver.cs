using System.IO;
using System.Diagnostics;
namespace FreeTorrserverBot
{
    public abstract class Torrserver
    {
        static string filePath = @"/opt/torrserver/accs.db";
        public static async Task ChangeAccountTorrserver()
        {
            var newParolRandom = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var newParol = new string(Enumerable.Repeat(chars, 8)
                         .Select(s => s[newParolRandom.Next(s.Length)]).ToArray());
            string result = $"{{\"freeServer\":\"{newParol}\"}}";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"{result}") ;
            }
          await  RebootingTorrserver();
        }
        public static string TakeAccountTorrserver()
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                string result = "";
                while ((line = reader.ReadLine()) != null)
                {
                    result+= line;
                    Console.WriteLine(line);
                }
                
                return result.Replace("\"","").Replace("{","").Replace("}","");
            }
        }
        public static async Task RebootingTorrserver()
        {
            Process.Start("killall", "torrserver");
            Process.Start("/opt/torrserver/torrserver");
            return;
        }
    }
}
