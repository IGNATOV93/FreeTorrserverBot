using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace AdTorrBot.BotTelegram.Db.Model.TorrserverModel
{


    public class BitTorrConfig
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public string IdChat { get; set; } = string.Empty;
        [JsonIgnore]
        [Description("Имя профиля")]
        public string NameProfileBot { get; set; } = "default";

        [Description("Размер кеша (кб)")]
        public long CacheSize { get; set; } = 67108864;

        [Description("Опережающий кеш (%)")]
        public int ReaderReadAHead { get; set; } = 95;

        [Description("Буфер предзагрузки (%)")]
        public int PreloadCache { get; set; } = 50;

        [Description("Использование диска (on/off)")]
        public bool UseDisk { get; set; } = false;

        [Description("Включение IPv6 (of/off)")]
        public bool EnableIPv6 { get; set; } = false;

        [Description("Отключение TCP (on/off)")]
        public bool DisableTCP { get; set; } = false;

        [Description("μTP (Micro Transport Protocol)")]
        public bool DisableUTP { get; set; } = false;

        [Description("Отключение PEX")]
        public bool DisablePEX { get; set; } = false;

        [Description("Принуд. шифрование (on/off)")]
        public bool ForceEncrypt { get; set; } = false;

        [Description("Тайм-аут откл. торрентов (сек.)")]
        public int TorrentDisconnectTimeout { get; set; } = 30;

        [Description("Торрент-соединения (кол-во)")]
        public int ConnectionsLimit { get; set; } = 25;

        [Description("Отключение DHT (on/off)")]
        public bool DisableDHT { get; set; } = false;

        [Description("Ограничение скорости загрузки (кб.)")]
        public int DownloadRateLimit { get; set; } = 0;

        [Description("Ограничение скорости отдачи (кб.)")]
        public int UploadRateLimit { get; set; } = 0;

        [Description("Порт для входящих подключений (число)")]
        public int PeersListenPort { get; set; } = 0;

        [Description("Отключение UPNP (on/off)")]
        public bool DisableUPNP { get; set; } = false;

        [Description("Включение DLNA (on/off)")]
        public bool EnableDLNA { get; set; } = false;

        [Description("Имя сервера DLNA (строка)")]
        public string FriendlyName { get; set; } = "";

        [Description("Вкл. поиск по RuTor (on/off)")]
        public bool EnableRutorSearch { get; set; } = false;

        [Description("Включение отладки (on/off)")]
        public bool EnableDebug { get; set; } = false;

        [Description("Включить быстрый режим чтения")]
        public bool ResponsiveMode { get; set; } = false;

        [Description("Режим ретрекеров (по выбору)")]
        public int RetrackersMode { get; set; } = 1;

        [Description("SSL порт (число)")]
        public int SslPort { get; set; } = 0;

        [Description("SSL сертификат (путь)")]
        public string SslCert { get; set; } = "";

        [Description("SSL ключ (путь)")]
        public string SslKey { get; set; } = "";

        [Description("Отключение отдачи (on/off)")]
        public bool DisableUpload { get; set; } = false;

        [Description("Удаление кеша при сбросе (on/off)")]
        public bool RemoveCacheOnDrop { get; set; } = false;

        [Description("Путь сохр. торрентов (путь)")]
        public string TorrentsSavePath { get; set; } = "";

        public override string ToString()
        {
            var properties = typeof(BitTorrConfig).GetProperties().Skip(2);
            var sb = new StringBuilder();
            var i = 0;
            foreach (var property in properties)
            {
                i++;
                var description = GetDescription(property.Name);
                var value = property.GetValue(this)?.ToString();
                sb.AppendLine($"{i}) {description} ➡ {value}\r\n");
            }

            return sb.ToString();
        }

        public string GetDescription(string propertyName)
        {
            var property = typeof(BitTorrConfig).GetProperty(propertyName);
            if (property == null) return null;

            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute));
            return descriptionAttribute?.Description;
        }
    }


}
