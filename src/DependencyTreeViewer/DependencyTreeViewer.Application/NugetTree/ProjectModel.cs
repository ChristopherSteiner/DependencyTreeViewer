namespace DependencyTreeViewer.Application.NugetTree;

public record ProjectModel
{
    public string Name { get; set; } = string.Empty;

    public string FullPath { get; set; } = string.Empty;

    public IEnumerable<FrameworkModel> TargetFrameworks { get; set; } = [];
}
