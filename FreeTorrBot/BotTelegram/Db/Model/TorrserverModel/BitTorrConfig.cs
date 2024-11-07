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


        [Description("Размер кеша (кб)")]
        public string CacheSize { get; set; } = "67108864";

        [Description("Опережающий кеш (%)")]
        public string ReaderReadAHead { get; set; } = "95";

        [Description("Буфер предзагрузки (%)")]
        public string PreloadCache { get; set; } = "50";

        [Description("Использование диска (on/off)")]
        public string UseDisk { get; set; } = "false";

        [Description("Включение IPv6 (of/off)")]
        public string EnableIPv6 { get; set; } = "false";

        [Description("Отключение TCP(on/off)")]
        public string DisableTCP { get; set; } = "false";
        [Description("μTP (Micro Transport Protocol)")]
        public string DisableUTP { get; set; } = "false";
        [Description("Отключение PEX")]
        public string DisablePEX { get; set; } = "false";
        [Description("Принуд. шифрование (on/off)")]
        public string ForceEncrypt { get; set; } = "false";
        [Description("Тайм-аут откл. торрентов (сек.)")]
        public string TorrentDisconnectTimeout { get; set; } = "30";

        [Description("Торрент-соединения (кол-во)")]
        public string ConnectionsLimit { get; set; } = "25";
        [Description("Отключение DHT (on/off)")]
        public string DisableDHT { get; set; } = "false";
        [Description("Ограничение скорости загрузки (кб.)")]
        public string DownloadRateLimit { get; set; } = "0";
        [Description("Ограничение скорости отдачи (кб.)")]
        public string UploadRateLimit { get; set; } = "0";
        [Description("Порт для входящих подключений (число)")]
        public string PeersListenPort { get; set; } = "0";

        [Description("Отключение UPNP (on/off)")]
        public string DisableUPNP { get; set; } = "false";
        [Description("Включение DLNA (on/off)")]
        public string EnableDLNA { get; set; } = "false";
        [Description("Имя сервера DLNA (строка)")]
        public string FriendlyName { get; set; } = "";
        [Description("Вкл. поиск по RuTor (on/off)")]
        public string EnableRutorSearch { get; set; } = "false";
        [Description("Включение отладки (on/off)")]
        public string EnableDebug { get; set; } = "false";

        [Description("Включить быстрый режим чтения")]
        public string ResponsiveMode { get; set; } = "false";
        [Description("Режим ретрекеров (по выбору)")]
        public string RetrackersMode { get; set; } = "1";
        [Description("SSL порт (число)")]
        public string SslPort { get; set; } = "0";
        [Description("SSL сертификат (путь)")]
        public string SslCert { get; set; } = "";
        [Description("SSL ключ (путь)")]
        public string SslKey { get; set; } = "";

        [Description("Отключение отдачи (on/off)")]
        public string DisableUpload { get; set; } = "false";
        [Description("Удаление кеша при сбросе (on/off)")]
        public string RemoveCacheOnDrop { get; set; } = "false";
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
                sb.AppendLine($"{i}) {description} \u27A1 {value}\r\n");
            }
            
            return sb.ToString();
        }
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
