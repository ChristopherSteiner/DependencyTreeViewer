using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DependencyTreeViewer.UI.Configuration;

public class OptionsWriter : IOptionsWriter
{
    private readonly IConfigurationRoot configuration;
    private readonly string filePath;

    public OptionsWriter(
        IConfigurationRoot configuration,
        string filePath)
    {
        this.configuration = configuration;
        this.filePath = filePath;
    }

    public async Task UpdateOptionsAsync(Action<JsonObject> callback)
    {
        string? path = Path.GetDirectoryName(filePath);

        ArgumentNullException.ThrowIfNull(path);

        if (!File.Exists(filePath))
        {
            File.Copy(Path.Combine(path, "appsettings.json"), filePath);
        }

        string config = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);

        JsonObject? serializedConfig = JsonSerializer.Deserialize<JsonObject>(config);

        callback(serializedConfig);

        using (FileStream stream = File.OpenWrite(filePath))
        {
            using (Utf8JsonWriter writer = new Utf8JsonWriter(stream))
            {
                stream.SetLength(0);
                serializedConfig.WriteTo(writer);
            }
        }

        configuration.Reload();
    }
}
