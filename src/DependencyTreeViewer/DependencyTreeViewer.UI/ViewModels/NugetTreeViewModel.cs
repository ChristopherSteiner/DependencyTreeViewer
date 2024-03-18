using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyTreeViewer.Application.Extensions;
using DependencyTreeViewer.Application.NugetTree;
using DependencyTreeViewer.UI.Configuration;
using DependencyTreeViewer.UI.Mappers;
using DependencyTreeViewer.UI.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace DependencyTreeViewer.UI.ViewModels;

public sealed partial class NugetTreeViewModel : ObservableObject, INavigationAware
{
    private bool isInitialized = false;

    private readonly INugetTreeService nugetTreeService;
    private readonly INugetTreeMapper nugetTreeMapper;
    private readonly IWritableOptions<AppConfig> appConfig;

    private Lazy<List<ProjectModel>> actualProjects;

    private const string DefaultCurrentSolutionText = "<Solution>";

    [ObservableProperty]
    private string currentSolution = DefaultCurrentSolutionText;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<ProjectDisplayModel> projects = [];

    public NugetTreeViewModel(
        INugetTreeService nugetTreeService,
        INugetTreeMapper nugetTreeMapper,
        IWritableOptions<AppConfig> appConfig)
    {
        this.nugetTreeService = nugetTreeService;
        this.nugetTreeMapper = nugetTreeMapper;
        this.appConfig = appConfig;
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
        isInitialized = true;
    }

    [RelayCommand]
    private void OnOpenSolutionPicker()
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "solution files (*.sln)|*.sln";
        if (fileDialog.ShowDialog() == true)
        {
            CurrentSolution = fileDialog.FileName;
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        BuildNugetTree();
    }

    partial void OnCurrentSolutionChanged(string? oldValue, string newValue)
    {
        UpdateActualProjectsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        BuildNugetTree();
    }

    private void BuildNugetTree()
    {
        if (!string.IsNullOrWhiteSpace(CurrentSolution) && CurrentSolution != DefaultCurrentSolutionText)
        {
            List<ProjectModel> projectModels = actualProjects.Value.Clone();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                projectModels = projectModels.Search(SearchText).ToList();
            }

            Projects = nugetTreeMapper.MapToProjectDisplayModels(projectModels);
        }
    }

    public async Task UpdateActualProjectsAsync()
    {
        if (!string.IsNullOrWhiteSpace(CurrentSolution) && CurrentSolution != DefaultCurrentSolutionText)
        {
            List<ProjectModel> projects = (await nugetTreeService.GetTreesAsync(CurrentSolution).ConfigureAwait(false)).ToList();
            actualProjects = new Lazy<List<ProjectModel>>(projects);

            appConfig.Value.RecentSolutions.AddUniqueWithLimit(CurrentSolution, appConfig.Value.MaxRecentSolutions);

            await appConfig.UpdateAsync(config =>
            {
                config.RecentSolutions = appConfig.Value.RecentSolutions;
            }).ConfigureAwait(false);
        }
    }
}
