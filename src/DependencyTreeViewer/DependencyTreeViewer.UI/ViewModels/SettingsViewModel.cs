using CommunityToolkit.Mvvm.ComponentModel;
using DependencyTreeViewer.Application.Extensions;
using DependencyTreeViewer.UI.Configuration;
using System;
using System.Reflection;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace DependencyTreeViewer.UI.ViewModels;

public sealed partial class SettingsViewModel(IWritableOptions<AppConfig> appConfig) : ObservableObject, INavigationAware
{
    private readonly IWritableOptions<AppConfig> appConfig = appConfig;

    private bool isInitialized = false;

    [ObservableProperty]
    private ApplicationTheme currentApplicationTheme = ApplicationTheme.Unknown;

    [ObservableProperty]
    private string appVersion = String.Empty;

    [ObservableProperty]
    private int maxRecentSolutions;

    [ObservableProperty]
    private bool displayRestartWarning;

    public void OnNavigatedTo()
    {
        if (!isInitialized)
        {
            InitializeViewModel();
        }
    }

    public void OnNavigatedFrom() { }

    partial void OnCurrentApplicationThemeChanged(ApplicationTheme oldValue, ApplicationTheme newValue)
    {
        ApplicationThemeManager.Apply(newValue);

        appConfig.UpdateAsync(config =>
        {
            config.Theme = newValue.ToString();
        }).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    partial void OnMaxRecentSolutionsChanged(int value)
    {
        if (isInitialized)
        {
            appConfig.UpdateAsync(config =>
            {
                config.MaxRecentSolutions = value;
                config.RecentSolutions = config.RecentSolutions.Limit(value);
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            DisplayRestartWarning = true;
        }
    }

    private void InitializeViewModel()
    {
        CurrentApplicationTheme = ApplicationThemeManager.GetAppTheme();
        AppVersion = $"{GetAssemblyVersion()}";
        MaxRecentSolutions = appConfig.Value.MaxRecentSolutions;

        ApplicationThemeManager.Changed += OnThemeChanged;

        isInitialized = true;
    }

    private void OnThemeChanged(ApplicationTheme currentApplicationTheme, Color systemAccent)
    {
        // Update the theme if it has been changed elsewhere than in the settings.
        if (CurrentApplicationTheme != currentApplicationTheme)
        {
            CurrentApplicationTheme = currentApplicationTheme;
        }
    }

    private static string GetAssemblyVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
    }
}
