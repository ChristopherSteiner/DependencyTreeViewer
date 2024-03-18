using System.Collections.ObjectModel;

namespace DependencyTreeViewer.UI.Models;

public class FrameworkDisplayModel
{
    public string Name { get; set; } = string.Empty;

    public ObservableCollection<PackageDisplayModel> References { get; set; } = [];
}
