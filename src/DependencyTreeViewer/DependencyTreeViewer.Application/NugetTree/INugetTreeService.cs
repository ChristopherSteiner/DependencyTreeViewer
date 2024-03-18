
namespace DependencyTreeViewer.Application.NugetTree;

public interface INugetTreeService
{
    Task<IEnumerable<ProjectModel>> GetTreesAsync(string solutionPath);
}