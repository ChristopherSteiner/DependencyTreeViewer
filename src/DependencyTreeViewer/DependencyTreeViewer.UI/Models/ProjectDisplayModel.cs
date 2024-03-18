using System.Collections.ObjectModel;

namespace DependencyTreeViewer.UI.Models;

public record ProjectDisplayModel
{
    public string Name { get; set; } = string.Empty;

    public ObservableCollection<FrameworkDisplayModel> Frameworks { get; set; } = [];
}
