namespace DependencyTreeViewer.Application.NugetTree;

public record PackageModel
{
    public string Name { get; set; } = string.Empty;

    public string ReferencedVersion { get; set; } = string.Empty;

    public string ActualVersion { get; set; } = string.Empty;

    public IEnumerable<PackageModel> References { get; set; } = [];
}
