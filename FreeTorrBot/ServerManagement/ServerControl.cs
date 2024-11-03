using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.ServerManagement
{
    public abstract class ServerControl
    {

        public static async Task SetBbrState(bool enable)
        {
            try
            {
                string path = "/etc/sysctl.conf";
                string content = File.ReadAllText(path);
                string bbrDef = "net.core.default_qdisc=fq";
                string bbrIpv4 = "net.ipv4.tcp_congestion_control=bbr";

                if (enable)
                {
                    // Удаляем старые строки, если они есть, чтобы избежать дубликатов, независимо от пробелов
                    content = System.Text.RegularExpressions.Regex.Replace(content, @"net\.core\.default_qdisc\s*=\s*fq", "");
                    content = System.Text.RegularExpressions.Regex.Replace(content, @"net\.ipv4\.tcp_congestion_control\s*=\s*bbr", "");
                    File.WriteAllText(path, content);

                    // Добавляем новые строки
                    await File.AppendAllTextAsync(path, "\n" + bbrDef);
                    await File.AppendAllTextAsync(path, "\n" + bbrIpv4);
                }
                else
                {
                    // Удаляем строки, если BBR выключен
                    content = System.Text.RegularExpressions.Regex.Replace(content, @"net\.core\.default_qdisc\s*=\s*fq", "");
                    content = System.Text.RegularExpressions.Regex.Replace(content, @"net\.ipv4\.tcp_congestion_control\s*=\s*bbr", "");
                    File.WriteAllText(path, content);
                }

                // Применяем настройки
                Process processSysctl = new Process();
                processSysctl.StartInfo.FileName = "sysctl";
                processSysctl.StartInfo.Arguments = "-p";
                processSysctl.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public static void RebootServer()
        {
            try
            {
                // Команда для перезагрузки сервера
                Process.Start("reboot"); return;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
