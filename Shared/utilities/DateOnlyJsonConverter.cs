using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.utilities
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            if (DateOnly.TryParse(dateString, out DateOnly result))
            {
                return result;
            }
            else
            {
                throw new JsonException($"Invalid DateOnly format: {dateString}");
            }
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());

        }
    }
}
