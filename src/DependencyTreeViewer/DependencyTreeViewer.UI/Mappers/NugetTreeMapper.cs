using DependencyTreeViewer.Application.NugetTree;
using DependencyTreeViewer.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DependencyTreeViewer.UI.Mappers;
public class NugetTreeMapper : INugetTreeMapper
{
    public ObservableCollection<ProjectDisplayModel> MapToProjectDisplayModels(IEnumerable<ProjectModel> projectModels)
    {
        return new ObservableCollection<ProjectDisplayModel>(projectModels.ToList().Select(MapToProjectDisplayModel));
    }

    private ProjectDisplayModel MapToProjectDisplayModel(ProjectModel project)
    {
        ProjectDisplayModel projectDisplayModel = new();
        projectDisplayModel.Name = project.Name;
        projectDisplayModel.Frameworks = MapToFrameworkDisplayModels(project.TargetFrameworks);

        return projectDisplayModel;
    }

    private ObservableCollection<FrameworkDisplayModel> MapToFrameworkDisplayModels(IEnumerable<FrameworkModel> frameworks)
    {
        return new ObservableCollection<FrameworkDisplayModel>(frameworks.ToList().Select(MapToFrameworkDisplayModel));
    }

    private FrameworkDisplayModel MapToFrameworkDisplayModel(FrameworkModel framework)
    {
        FrameworkDisplayModel frameworkDisplayModel = new();
        frameworkDisplayModel.Name = framework.Name;
        frameworkDisplayModel.References = MapToPackageDisplayModels(framework.References);

        return frameworkDisplayModel;
    }

    private ObservableCollection<PackageDisplayModel> MapToPackageDisplayModels(IEnumerable<PackageModel> references)
    {
        return new ObservableCollection<PackageDisplayModel>(references.ToList().Select(MapToPackageDisplayModel));
    }

    private PackageDisplayModel MapToPackageDisplayModel(PackageModel model)
    {
        PackageDisplayModel packageDisplayModel = new();
        packageDisplayModel.Name = model.Name;
        packageDisplayModel.ReferencedVersion = model.ReferencedVersion;
        packageDisplayModel.ActualVersion = model.ActualVersion;
        packageDisplayModel.References = MapToPackageDisplayModels(model.References);

        return packageDisplayModel;
    }
}
