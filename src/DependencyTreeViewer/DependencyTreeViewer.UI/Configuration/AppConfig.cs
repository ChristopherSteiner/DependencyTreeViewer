using System.Collections.Generic;

namespace DependencyTreeViewer.UI.Configuration;

public class AppConfig
{
    public string? Theme { get; set; }

    public int MaxRecentSolutions { get; set; }

    public List<string> RecentSolutions { get; set; } = [];
}
