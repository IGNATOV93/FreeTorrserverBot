using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeTorrBot.BotTelegram.BotSettings.Model;

namespace FreeTorrBot.BotTelegram.BotSettings
{
    public  abstract class BotSettingsMethods
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static BotSettingsJson LoadSettings()
        {
           

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<BotSettingsJson>(json);
        }
        public static void SaveSettings(BotSettingsJson settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
