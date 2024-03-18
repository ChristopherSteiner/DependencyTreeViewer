
namespace DependencyTreeViewer.Application.ProjectAsset;

public interface IProjectAssetService
{
    Task<ProjectAssetsJsonModel> ReadProjectAssetAsync(string fullProjectPath);
}