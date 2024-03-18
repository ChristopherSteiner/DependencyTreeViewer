using DependencyTreeViewer.Application.NugetTree;
using DependencyTreeViewer.Application.ProjectAsset;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyTreeViewer.Application.Extensions;

public static class ServicesCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<INugetTreeService, NugetTreeService>();
        services.AddTransient<IProjectAssetService, ProjectAssetService>();
    }
}
