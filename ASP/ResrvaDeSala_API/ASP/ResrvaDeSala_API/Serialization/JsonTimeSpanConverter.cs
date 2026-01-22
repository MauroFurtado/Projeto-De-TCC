using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResrvaDeSala_API.Serialization;

public sealed class JsonTimeSpanConverter : JsonConverter<TimeSpan>
{
    private static readonly string[] Formats =
    {
        "c",
        @"hh\:mm",
        @"hh\:mm\:ss"
    };

    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string value for TimeSpan.");
        }

        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new JsonException("TimeSpan value cannot be empty.");
        }

        if (TimeSpan.TryParseExact(value, Formats, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        throw new JsonException($"Value '{value}' is not a valid TimeSpan.");
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(@"hh\:mm"));
    }
}
