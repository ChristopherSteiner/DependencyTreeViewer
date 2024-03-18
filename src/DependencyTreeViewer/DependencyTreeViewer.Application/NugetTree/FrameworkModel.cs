namespace DependencyTreeViewer.Application.NugetTree;

public class FrameworkModel
{
    public string Name { get; set; } = string.Empty;

    public IEnumerable<PackageModel> References { get; set; } = [];
}
