using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyTreeViewer.UI.Configuration;
using DependencyTreeViewer.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace DependencyTreeViewer.UI.ViewModels;

public sealed partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private bool isInitialized = false;
    private readonly IOptions<AppConfig> appConfig;
    private readonly INavigationService navigationService;
    private readonly IServiceProvider serviceProvider;

    [ObservableProperty]
    private List<string> recentSolutions = [];

    public DashboardViewModel(
        IOptions<AppConfig> appConfig,
        INavigationService navigationService,
        IServiceProvider serviceProvider)
    {
        this.appConfig = appConfig;
        this.navigationService = navigationService;
        this.serviceProvider = serviceProvider;
    }

    public void OnNavigatedTo()
    {
        if (!isInitialized)
        {
            InitializeViewModel();
        }
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel()
    {
        if (!string.IsNullOrWhiteSpace(appConfig.Value.Theme) && Enum.TryParse(appConfig.Value.Theme, out ApplicationTheme theme))
        {
            ApplicationThemeManager.Apply(theme);
        }

        RecentSolutions = appConfig.Value.RecentSolutions;

        isInitialized = true;
    }

    [RelayCommand]
    private void OnOpenSolution(string path)
    {
        NugetTreeViewModel viewModel = serviceProvider.GetRequiredService<NugetTreeViewModel>();
        viewModel.CurrentSolution = path;

        navigationService.Navigate(nameof(NugetTreePage));
    }


    [RelayCommand]
    private void OnOpenFolder(string path)
    {
        string? directory = Path.GetDirectoryName(path);
        if (Directory.Exists(directory))
        {
            Process.Start("explorer", directory);
        }
    }
}
