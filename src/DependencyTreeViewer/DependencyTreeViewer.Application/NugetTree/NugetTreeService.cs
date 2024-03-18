using DependencyTreeViewer.Application.ProjectAsset;
using Microsoft.Build.Construction;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DependencyTreeViewer.Application.NugetTree;

public class NugetTreeService(IProjectAssetService projectAssetService) : INugetTreeService
{
    /// <summary>
    /// Returns the project tree and its nuget dependencyies of a given solution
    /// </summary>
    /// <param name="solutionPath">The solution whose tree to display</param>    
    public async Task<IEnumerable<ProjectModel>> GetTreesAsync(string solutionPath)
    {
        Stopwatch sw = Stopwatch.StartNew();

        List<ProjectModel> projects = GetProjectsFromSolutionFile(solutionPath);
        // TODO Packages auslesen, später SelectConcurrent implementieren?
        IEnumerable<Task<ProjectModel>> projectTasks = projects.Select(CreateTreeFromProjectAsync);
        await Task.WhenAll(projectTasks).ConfigureAwait(false);

        Debug.WriteLine($"{sw.ElapsedMilliseconds}ms for completion of {nameof(GetTreesAsync)}");
        sw.Reset();
        return projects;
    }

    private async Task<ProjectModel> CreateTreeFromProjectAsync(ProjectModel project)
    {
        ProjectAssetsJsonModel projectAsset = await projectAssetService.ReadProjectAssetAsync(project.FullPath).ConfigureAwait(false);

        List<FrameworkModel> packages = new List<FrameworkModel>();
        project.TargetFrameworks = BuildFrameworkDependencyTreeFromProjectAssets(projectAsset);

        return project;
    }

    private List<ProjectModel> GetProjectsFromSolutionFile(string solutionPath)
    {
        SolutionFile solution = SolutionFile.Parse(solutionPath);
        return solution.ProjectsInOrder
            .Where(project => project.ProjectType is SolutionProjectType.KnownToBeMSBuildFormat or SolutionProjectType.WebProject)
            .Select(project =>
            {
                ProjectModel projectModel = new();
                projectModel.FullPath = project.AbsolutePath;
                projectModel.Name = Path.GetFileName(project.AbsolutePath);
                return projectModel;
            }).ToList();
    }

    private List<FrameworkModel> BuildFrameworkDependencyTreeFromProjectAssets(ProjectAssetsJsonModel projectAsset)
    {
        List<FrameworkModel> frameworkModels = [];

        foreach (KeyValuePair<string, FrameworkJsonModel> frameworkJson in projectAsset.Project.Frameworks)
        {
            FrameworkModel frameworkModel = new();
            frameworkModel.Name = frameworkJson.Key;
            frameworkModel.References = BuildPackageDependencyTreeFromProjectAssets(projectAsset, frameworkJson.Key, frameworkJson.Value);

            frameworkModels.Add(frameworkModel);
        }

        return frameworkModels;
    }

    private List<PackageModel> BuildPackageDependencyTreeFromProjectAssets(ProjectAssetsJsonModel projectAsset, string frameworkName, FrameworkJsonModel framework)
    {
        List<PackageModel> packageModels = [];
        Dictionary<string, TargetJsonModel> frameworkDependencies = projectAsset.Targets[MapFrameworkNameToTarget(frameworkName)];

        foreach (KeyValuePair<string, DependencyJsonModel> dependency in framework.Dependencies)
        {
            PackageModel packageModel = new();
            packageModel.Name = dependency.Key;
            packageModel.ReferencedVersion = dependency.Value.Version;
            packageModel.ActualVersion = dependency.Value.Version;
            packageModel.References = BuildPackageDependencyTreeFromDependencies(frameworkDependencies, dependency.Key, dependency.Value.Version, out string _);

            packageModels.Add(packageModel);
        }

        return packageModels;
    }

    private List<PackageModel> BuildPackageDependencyTreeFromDependencies(Dictionary<string, TargetJsonModel> frameworkDependencies, string targetName, string targetVersion, out string actualVersion)
    {
        List<PackageModel> packageModels = [];
        KeyValuePair<string, TargetJsonModel> target = frameworkDependencies.First(dependency => dependency.Key.StartsWith(targetName));
        actualVersion = GetVersionFromTargetKey(target.Key);

        foreach (KeyValuePair<string, string> dependency in target.Value.Dependencies)
        {
            PackageModel packageModel = new();
            packageModel.Name = dependency.Key;
            packageModel.ReferencedVersion = dependency.Value;

            packageModel.References = BuildPackageDependencyTreeFromDependencies(frameworkDependencies, dependency.Key, dependency.Value, out string currentActualVersion);
            packageModel.ActualVersion = currentActualVersion;

            packageModels.Add(packageModel);
        }

        return packageModels;
    }

    private string MapFrameworkNameToTarget(string frameworkName)
    {
        Match coreMatch = Regex.Match(frameworkName, "(netcoreapp|netstandard)(\\d\\.?\\d)");
        if (coreMatch.Success)
        {
            return $".{coreMatch.Groups[1]},Version=v{coreMatch.Groups[2]}";
        }

        return frameworkName;
    }

    private string GetVersionFromTargetKey(string targetKey)
    {
        Match versionMatch = Regex.Match(targetKey, ".*\\/(.*)");
        if (versionMatch.Success)
        {
            return versionMatch.Groups[1].Value;
        }

        throw new ArgumentException($"Could not receive version from string {targetKey}");
    }
}
