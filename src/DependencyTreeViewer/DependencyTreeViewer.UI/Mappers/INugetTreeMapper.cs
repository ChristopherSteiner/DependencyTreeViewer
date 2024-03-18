using DependencyTreeViewer.Application.NugetTree;
using DependencyTreeViewer.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DependencyTreeViewer.UI.Mappers;

public interface INugetTreeMapper
{
    ObservableCollection<ProjectDisplayModel> MapToProjectDisplayModels(IEnumerable<ProjectModel> projectModels);
}