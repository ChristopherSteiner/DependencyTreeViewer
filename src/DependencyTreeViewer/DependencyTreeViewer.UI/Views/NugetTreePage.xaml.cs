using DependencyTreeViewer.UI.Helpers;
using DependencyTreeViewer.UI.ViewModels;
using Wpf.Ui.Controls;

namespace DependencyTreeViewer.UI.Views;

/// <summary>
/// Interaction logic for NugetTreePage.xaml
/// </summary>
public partial class NugetTreePage : INavigableView<NugetTreeViewModel>
{
    public NugetTreeViewModel ViewModel { get; }

    public NugetTreePage(NugetTreeViewModel viewModel)
    {
        ViewModel = viewModel;
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        DataContext = this;

        InitializeComponent();
    }

    /// <summary>
    /// For the tree view to expand all after altering the solution or searchtext.
    /// Note: For some reason it doesn't work properly in the ui when instead for the Projects property is listened
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ViewModel.SearchText) or nameof(ViewModel.CurrentSolution))
        {
            tvNugetTree.ExpandAll();
        }
    }

    /// <summary>
    /// Expand the tree also when the page is loaded the first time. Because if we open a solution from the dashboard the PropertyChanged
    /// Event is not yet subscribed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        tvNugetTree.ExpandAll();
    }
}
