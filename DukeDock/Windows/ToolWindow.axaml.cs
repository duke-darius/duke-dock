using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DukeDock.Controls;
using DukeDock.Lib;
using DukeDock.Windows.OtpWindows;
using DynamicData;
using JetBrains.Annotations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DukeDock.Windows;

public partial class ToolWindow : Window
{
    private string? _searchText;
    private SearchResult? _selectedSearchResult;
    

    [Reactive]
    public string? SearchText
    {
        get => _searchText;
        [UsedImplicitly]
        set
        {
            _searchText = value;
            UpdateSearch();
        }
    }

    public ToolWindow()
    {
        InitializeComponent();
        DataContext = this;
        
        UpdateSearch();

    }

    public ReactiveCommand<Unit, Unit> OtpClicked { get; } = ReactiveCommand.Create(() =>
    {
        Dispatcher.UIThread.Post(() =>
        {
            var win = new OtpWindow();
            win.Show();
            App.MainWindow.Hide();
        });
    });

    public ReactiveCommand<Unit, Unit> StringStoreClicked { get; } = ReactiveCommand.Create(() =>
    {
        Dispatcher.UIThread.Post(() =>
        {
            var win = new StringStoreWindow();
            win.Show();
            App.MainWindow.Hide();
        });
    });

    public void ToggleVisibility()
    {
        Dispatcher.UIThread.Post(() =>
        {
            if (IsVisible)
                Hide();
            else
                Show();
        });

    }

    private void Window_OnClosing(object? sender, CancelEventArgs e)
    {

    }

    private void WindowBase_OnDeactivated(object? sender, EventArgs e)
    {
        Close();
    }

    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        App.CurrentToolWindow = null;
    }

    private void UpdateSearch()
    {
        Dispatcher.UIThread.Post(() =>
        {
            ResultsBox.Children.Clear();

            var results = App.Features.Where(x =>
                x.FeatureName.Contains(SearchText ?? "", StringComparison.CurrentCultureIgnoreCase));
            foreach (var result in results)
            {
                ResultsBox.Children.Add(new SearchResult(result));
            }
            _selectedSearchResult = ResultsBox.Children.FirstOrDefault() as SearchResult;
            _selectedSearchResult?.SetActive(true);
        });
        
    }

    private void WindowBase_OnActivated(object? sender, EventArgs e)
    {
        SearchTextBox.Focus();
    }

    private void SearchTextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if(!ResultsBox.Children.Any())
            return;

        var results = ResultsBox.Children.Cast<SearchResult>().ToArray();
        if (e.Key is Key.Up or Key.Down)
        {
            var currentIndex = results.IndexOf(_selectedSearchResult);
            currentIndex += e.Key == Key.Up ? -1 : 1;
            foreach (var result in results)
            {
                result.SetActive(false);
            }

            if (currentIndex >= results.Length)
                currentIndex = 0;
            if (currentIndex < 0)
                currentIndex = results.Length - 1;

            _selectedSearchResult = results[currentIndex];
            _selectedSearchResult.SetActive(true);
        }

        if (e.Key is Key.Enter)
        {
            if(_selectedSearchResult == null || _selectedSearchResult.Feature == null)
                return;
            var feature = _selectedSearchResult.Feature;
            var window = (FeatureWindow)Activator.CreateInstance(feature.WindowType)!;
            window.Show();
            Close();
        }
    }
}