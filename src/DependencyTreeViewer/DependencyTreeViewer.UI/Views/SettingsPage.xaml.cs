using DependencyTreeViewer.UI.ViewModels;
using Wpf.Ui.Controls;

namespace DependencyTreeViewer.UI.Views;

/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage : INavigableView<SettingsViewModel>
{
    public SettingsViewModel ViewModel { get; }

    public SettingsPage(SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
