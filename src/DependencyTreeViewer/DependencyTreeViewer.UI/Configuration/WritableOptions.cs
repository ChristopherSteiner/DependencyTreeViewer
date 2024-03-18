using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DependencyTreeViewer.UI.Configuration;
public class WritableOptions<TOption> : IWritableOptions<TOption> where TOption : class, new()
{
    private readonly string sectionName;
    private readonly IOptionsWriter writer;
    private readonly IOptionsMonitor<TOption>? options;

    public WritableOptions(
        string sectionName,
        IOptionsWriter writer,
        IOptionsMonitor<TOption>? options)
    {
        this.sectionName = sectionName;
        this.writer = writer;
        this.options = options;
    }

    public TOption Value => options?.CurrentValue ?? new TOption();

    public async Task UpdateAsync(Action<TOption> applyChanges)
    {
        await writer.UpdateOptionsAsync(opt =>
        {
            JsonNode? section = opt[sectionName];
            TOption? sectionObject = section == null ?
                options?.CurrentValue :
                JsonSerializer.Deserialize<TOption>(section.ToString());

            if (sectionObject == null)
            {
                sectionObject = new TOption();
            }

            applyChanges(sectionObject);

            string json = JsonSerializer.Serialize(sectionObject);
            opt[sectionName] = JsonObject.Parse(json);
        }).ConfigureAwait(false);
    }
}
