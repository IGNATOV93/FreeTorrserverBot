using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace AdTorrBot.ServerManagement
{
    public class ServerInfo
    {


        public static bool CheckBBRConfig()
        {
            string path = "/etc/sysctl.conf";
            if (!File.Exists(path))
            {
                return false;
            }

            string content = File.ReadAllText(path);

            // Игнорируем пробелы при проверке строки
            bool hasCongestionControl = Regex.IsMatch(
                content,
                @"net\.ipv4\.tcp_congestion_control\s*=\s*bbr"
            );

            return hasCongestionControl;
        }



        public static double GetLocalServerTimeTimeZone()
        {
            // Получаем текущее время на сервере
            DateTime localTime = DateTime.Now;

            // Получаем информацию о текущем часовом поясе
            TimeZoneInfo localZone = TimeZoneInfo.Local;

            // Получаем смещение от UTC
            TimeSpan offset = localZone.GetUtcOffset(localTime);

            // Возвращаем смещение в виде double
            return offset.Hours + offset.Minutes / 60.0; // Приводим минуты к часам
        }
        public static string GetLocalServerTime()
        {
            return DateTime.Now.ToString("HH:mm"); // Возвращает текущее локальное время на сервере
        }
    }
}
