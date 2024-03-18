using System;
using System.Windows;
using Wpf.Ui;

namespace DependencyTreeViewer.UI;

/// <summary>
/// Creates new instance and attaches the <see cref="IServiceProvider"/>.
/// </summary>
public class PageService(IServiceProvider serviceProvider) : IPageService
{
    /// <summary>
    /// Service which provides the instances of pages.
    /// </summary>
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public T? GetPage<T>()
        where T : class
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return (T?)_serviceProvider.GetService(typeof(T));
    }

    public FrameworkElement? GetPage(Type pageType)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return _serviceProvider.GetService(pageType) as FrameworkElement;
    }
}
