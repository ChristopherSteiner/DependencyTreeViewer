using System.Text.Json;
using System.Text.Json.Serialization;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class CaseInsensitiveDictionaryConverter<TValue>
    : JsonConverter<Dictionary<string, TValue>>
{
    public override Dictionary<string, TValue> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        Dictionary<string, TValue>? dic = (Dictionary<string, TValue>?)JsonSerializer.Deserialize(ref reader, typeToConvert, options);

        dic ??= [];

        return new Dictionary<string, TValue>(dic, StringComparer.OrdinalIgnoreCase);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Dictionary<string, TValue> value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(
            writer, value, value.GetType(), options);
    }
}