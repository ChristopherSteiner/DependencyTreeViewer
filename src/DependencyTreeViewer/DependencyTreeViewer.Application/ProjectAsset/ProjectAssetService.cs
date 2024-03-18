using System.Text.Json;

namespace DependencyTreeViewer.Application.ProjectAsset;

public class ProjectAssetService : IProjectAssetService
{
    private const string ObjectFolder = "obj";
    private const string ProjectAssetsFileName = "project.assets.json";

    public async Task<ProjectAssetsJsonModel> ReadProjectAssetAsync(string fullProjectPath)
    {
        string projectAssetsPath = Path.Combine(Path.GetDirectoryName(fullProjectPath) ?? string.Empty, ObjectFolder, ProjectAssetsFileName);
        if (File.Exists(projectAssetsPath))
        {
            string json = await File.ReadAllTextAsync(projectAssetsPath).ConfigureAwait(false);
            ProjectAssetsJsonModel? projectAssets = JsonSerializer.Deserialize<ProjectAssetsJsonModel>(json);
            if (projectAssets != null)
            {
                return projectAssets;
            }

        }

        return new ProjectAssetsJsonModel();
    }
}
