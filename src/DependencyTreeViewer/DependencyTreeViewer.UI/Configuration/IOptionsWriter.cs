using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DependencyTreeViewer.UI.Configuration;

public interface IOptionsWriter
{
    Task UpdateOptionsAsync(Action<JsonObject> callback);
}