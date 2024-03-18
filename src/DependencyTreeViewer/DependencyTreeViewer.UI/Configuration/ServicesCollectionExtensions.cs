using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DependencyTreeViewer.UI.Configuration;

public static class ServicesCollectionExtensions
{
    public static void ConfigureWritable<T>(
        this IServiceCollection services,
        IConfigurationRoot configuration,
        string sectionName,
        string file) where T : class, new()
    {
        services.Configure<T>(configuration.GetSection(sectionName));

        services.AddTransient<IWritableOptions<T>>(provider =>
        {
            IOptionsMonitor<T>? options = provider.GetService<IOptionsMonitor<T>>();
            IOptionsWriter writer = new OptionsWriter(configuration, file);
            return new WritableOptions<T>(sectionName, writer, options);
        });
    }
}
