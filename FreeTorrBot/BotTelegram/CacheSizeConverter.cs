using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace AdTorrBot.BotTelegram
{
    public class CacheSizeConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Значение в JSON задается в байтах, конвертируется в мегабайты
            return reader.GetInt64() / (1024 * 1024);
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            // Значение в объекте задается в мегабайтах, конвертируется в байты
            writer.WriteNumberValue(value * 1024 * 1024);
        }
    }
}
