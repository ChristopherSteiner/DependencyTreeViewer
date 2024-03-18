using CommunityToolkit.Mvvm.ComponentModel;
using DependencyTreeViewer.UI.Views;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace DependencyTreeViewer.UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private bool isInitialized = false;

    [ObservableProperty]
    private string applicationTitle = String.Empty;

    [ObservableProperty]
    private ObservableCollection<object> navigationItems = [];

    [ObservableProperty]
    private ObservableCollection<object> navigationFooter = [];

    [ObservableProperty]
    private ObservableCollection<MenuItem> trayMenuItems = [];

    public MainWindowViewModel()
    {
        if (!isInitialized)
        {
            InitializeViewModel();
        }

    }

    private void InitializeViewModel()
    {
        ApplicationTitle = App.ApplicationName;

        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Nuget Tree",
                TargetPageTag = nameof(NugetTreePage),
                Icon = new SymbolIcon { Symbol = SymbolRegular.Box24 },
                TargetPageType = typeof(NugetTreePage)
            }
        ];

        NavigationFooter =
        [
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(SettingsPage)
            }
        ];

        TrayMenuItems =
        [
            new() { Header = "Home", Tag = "tray_home" }
        ];

        isInitialized = true;
    }
}
