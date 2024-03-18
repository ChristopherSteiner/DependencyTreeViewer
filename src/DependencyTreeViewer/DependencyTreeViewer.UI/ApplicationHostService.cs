using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui;

namespace DependencyTreeViewer.UI;

public class ApplicationHostService(IServiceProvider serviceProvider) : IHostedService
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private INavigationWindow? navigationWindow;

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates main window during activation.
    /// </summary>
    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!System.Windows.Application.Current.Windows.OfType<MainWindow>().Any())
        {
            navigationWindow = (
                serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow
            )!;
            navigationWindow!.ShowWindow();

            navigationWindow.Navigate(typeof(Views.DashboardPage));
        }

        await Task.CompletedTask;
    }
}
