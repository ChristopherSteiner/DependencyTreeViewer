using System.Text.Json.Serialization;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class FrameworkJsonModel
{
    [JsonPropertyName("dependencies")]
    public Dictionary<string, DependencyJsonModel> Dependencies { get; set; } = [];
}
