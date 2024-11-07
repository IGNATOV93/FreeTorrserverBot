using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace AdTorrBot.BotTelegram.Db.Model.TorrserverModel
{
  

    public class BitTorrConfig
    {

        [Key]
        public int Id { get; set; }
   
        public string IdChat { get; set; } = string.Empty;

        [Description("Имя профиля")]
        public string NameProfileBot { get; set; } = "default";

        [Description("Размер кеша")]
        public string CacheSize { get; set; } = "67108864";

        [Description("Лимит соединений")]
        public string ConnectionsLimit { get; set; } = "25";

        [Description("Отключение DHT")]
        public string DisableDHT { get; set; } = "false";

        [Description("Отключение PEX")]
        public string DisablePEX { get; set; } = "false";

        [Description("Отключение TCP")]
        public string DisableTCP { get; set; } = "false";

        [Description("Отключение UPNP")]
        public string DisableUPNP { get; set; } = "false";

        [Description("Отключение UTP")]
        public string DisableUTP { get; set; } = "false";

        [Description("Отключение \uD83D\uDCE5")]
        public string DisableUpload { get; set; } = "false";

        [Description("Лимит скорости \uD83D\uDCE5")]
        public string DownloadRateLimit { get; set; } = "0";

        [Description("Включение DLNA")]
        public string EnableDLNA { get; set; } = "false";

        [Description("Включение отладки")]
        public string EnableDebug { get; set; } = "false";

        [Description("Включение IPv6")]
        public string EnableIPv6 { get; set; } = "false";

        [Description("Вкл. \uD83D\uDD0E Rutor")]
        public string EnableRutorSearch { get; set; } = "false";

        [Description("Принуд. шифрование")]
        public string ForceEncrypt { get; set; } = "false";

        [Description("Дружественное имя")]
        public string FriendlyName { get; set; } = "";

        [Description("Порт просл. пиров")]
        public string PeersListenPort { get; set; } = "0";

        [Description("Предв.  📥 кеша")]
        public string PreloadCache { get; set; } = "50";

        [Description("Чтение вперед")]
        public string ReaderReadAHead { get; set; } = "95";

        [Description("\uD83D\uDDD1 Кеша при сбросе")]
        public string RemoveCacheOnDrop { get; set; } = "false";

        [Description("Режим отклика")]
        public string ResponsiveMode { get; set; } = "false";

        [Description("Режим ретрекеров")]
        public string RetrackersMode { get; set; } = "1";

        [Description("SSL сертификат")]
        public string SslCert { get; set; } = "";

        [Description("SSL ключ")]
        public string SslKey { get; set; } = "";

        [Description("SSL порт")]
        public string SslPort { get; set; } = "0";

        [Description("\u231B Откл торрентов")]
        public string TorrentDisconnectTimeout { get; set; } = "30";

        [Description("\uD83D\uDDC2 Сохр. торрентов")]
        public string TorrentsSavePath { get; set; } = "";

        [Description("Лимит скорости \uD83D\uDCE4")]
        public string UploadRateLimit { get; set; } = "0";

        [Description("Использ. диска")]
        public string UseDisk { get; set; } = "false";

        public string GetDescription(string propertyName)
        {
            var property = typeof(BitTorrConfig).GetProperty(propertyName);
            if (property == null)
            {
                return null;
            }

            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute));
            return descriptionAttribute?.Description;
        }
    }

}
