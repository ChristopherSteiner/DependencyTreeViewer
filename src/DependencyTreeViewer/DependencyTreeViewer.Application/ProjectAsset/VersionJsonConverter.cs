using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class VersionJsonConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (!string.IsNullOrWhiteSpace(value))
        {
            return Regex.Replace(value, "[^0-9a-zA-Z.]+", "");
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}
