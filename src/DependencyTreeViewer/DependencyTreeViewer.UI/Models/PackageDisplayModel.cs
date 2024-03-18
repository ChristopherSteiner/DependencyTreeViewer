using System.Collections.ObjectModel;

namespace DependencyTreeViewer.UI.Models;
public record PackageDisplayModel
{
    public string Name { get; set; } = string.Empty;

    public string ReferencedVersion { get; set; } = string.Empty;

    public string ActualVersion { get; set; } = string.Empty;

    public ObservableCollection<PackageDisplayModel> References { get; set; } = [];
}
