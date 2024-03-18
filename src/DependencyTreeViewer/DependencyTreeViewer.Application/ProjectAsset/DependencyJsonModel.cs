using System.Text.Json.Serialization;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class DependencyJsonModel
{
    [JsonPropertyName("target")]
    public string Target { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    [JsonConverter(typeof(VersionJsonConverter))]
    public string Version { get; set; } = string.Empty;
}