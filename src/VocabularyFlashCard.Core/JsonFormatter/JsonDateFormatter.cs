using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VocabularyFlashCard.Core.JsonFormatter;

/// <summary>
/// Json formatter for custom date style
/// like: 2020-02-20 or etc
/// </summary>
class JsonDateFormatter : JsonConverter<DateTime>
{
	private const string _format = "yyyy-MM-dd HH:mm:ss.ff"; // SQL Server, datetime2
	private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var dateString = reader.GetString() ?? string.Empty;

		if (DateTime.TryParseExact(dateString,
								_format,
								_culture,
								DateTimeStyles.None,
								out DateTime result))
		{
			return result;
		}
		else
		{
			return default;
		}
	}


	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
   => writer.WriteStringValue(value.ToString(_format, _culture));
}
