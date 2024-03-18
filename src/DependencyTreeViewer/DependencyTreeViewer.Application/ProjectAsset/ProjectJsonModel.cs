using System.Text.Json.Serialization;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class ProjectJsonModel
{
    [JsonPropertyName("frameworks")]
    public Dictionary<string, FrameworkJsonModel> Frameworks { get; set; } = [];
}
