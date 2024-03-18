using System.Text.Json.Serialization;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class ProjectAssetsJsonModel
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("targets")]
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<Dictionary<string, TargetJsonModel>>))]
    public Dictionary<string, Dictionary<string, TargetJsonModel>> Targets { get; set; } = [];

    [JsonPropertyName("project")]
    public ProjectJsonModel Project { get; set; } = new();
}
