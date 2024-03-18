using System.Text.Json.Serialization;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class TargetJsonModel
{
    [JsonPropertyName("dependencies")]
    public Dictionary<string, string> Dependencies { get; set; } = [];
}
