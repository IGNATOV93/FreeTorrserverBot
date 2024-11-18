using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram.Db.Model
{
    public class TextInputFlag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? IdChat { get; set; }

        public string? LastTextFlagTrue { get; set; }
        [Description($"Ввод: Логин Torrserver")]
        public bool FlagLogin { get; set; }=false;
        [Description($"Ввод: пароль Torrserver")]
        public bool FlagPassword { get; set; }=false;


        [Description($"Ввод: Размер кеша мб.")]
        public bool FlagTorrSettCacheSize { get; set; } = false;

        [Description("Ввод: Опережающий кеш %")]
        public bool FlagTorrSettReaderReadAHead { get; set; } = false;

        [Description("Ввод: Буфер предзагрузки %")]
        public bool FlagTorrSettPreloadCache { get; set; } = false;

        [Description("Ввод: Тайм-аут откл. торрентов сек.")]
        public bool FlagTorrSettTorrentDisconnectTimeout { get; set; } = false;

        [Description("Ввод: Торрент-соединения кол-во")]
        public bool FlagTorrSettConnectionsLimit { get; set; } = false;

        [Description("Ввод: Ограничение скорости загрузки кб.")]
        public bool FlagTorrSettDownloadRateLimit { get; set; } = false;

        [Description("Ввод: Ограничение скорости отдачи кб.")]
        public bool FlagTorrSettUploadRateLimit { get; set; } = false;

        [Description("Ввод: Порт для входящих подключений")]
        public bool FlagTorrSettPeersListenPort { get; set; } = false;

        [Description("Ввод: Имя сервера DLNA")]
        public bool FlagTorrSettFriendlyName { get; set; } = false;

        [Description("Ввод: Режим ретрекеров")]
        public bool FlagTorrSettRetrackersMode { get; set; } = false;

        [Description("Ввод: SSL порт")]
        public bool FlagTorrSettSslPort { get; set; } = false;

        [Description("Ввод: SSL сертификат")]
        public bool FlagTorrSettSslCert { get; set; } = false;

        [Description("Ввод: SSL ключ")]
        public bool FlagTorrSettSslKey { get; set; } = false;

        [Description("Ввод: Путь сохр. торрентов")]
        public bool FlagTorrSettTorrentsSavePath { get; set; } = false;

        public bool CheckAllBooleanFlags()
        {
            var boolProperties = this.GetType()
                                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => p.PropertyType == typeof(bool));

            foreach (var property in boolProperties)
            {
                if ((bool)property.GetValue(this) == true)
                {
                    return true; // Возвращает true, если найдено хотя бы одно значение true
                }
            }

            return false; // Возвращает false, если все булевые значения равны false
        }
    }
}
