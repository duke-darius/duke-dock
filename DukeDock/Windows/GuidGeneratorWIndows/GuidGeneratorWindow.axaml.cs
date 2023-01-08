using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DukeDock.Lib;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DukeDock.Windows.GuidGeneratorWIndows;

public partial class GuidGeneratorWindow : FeatureWindow
{
    public Guid NewGuid { get; set; }

    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public ReactiveCommand<Unit, Unit> CopyCommand { get; }

    public string? CurrentSelection { get; set; } = null!;
    
    [Reactive] public ObservableCollection<string> GuidFormats { get; } = new();

    public GuidGeneratorWindow()
    {
        ExitCommand = ReactiveCommand.Create(Close);
        CopyCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await (Application.Current?.Clipboard?.SetTextAsync(CurrentSelection ?? "") ?? Task.CompletedTask);
            Close();

        });
        
        App.OpenWindows.Add(this);
        NewGuid = Guid.NewGuid();
        
        InitializeComponent();
        DataContext = this;
        
        GuidFormats.Add(NewGuid.ToString());
        GuidFormats.Add(NewGuid.ToString().ToUpper());
        GuidFormats.Add(NewGuid.ToString().Replace("-", ""));
        GuidFormats.Add(NewGuid.ToString().ToUpper().Replace("-", ""));
        GuidFormats.Add($"[{NewGuid.ToString()}]");
        GuidFormats.Add($"[{NewGuid.ToString().ToUpper()}]");


    }
    
    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        App.OpenWindows.Remove(this);
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        CurrentSelection = GuidListBox?.Selection?.SelectedItem as string;
    }

    private void TopLevel_OnOpened(object? sender, EventArgs e)
    {
        GuidListBox.ItemContainerGenerator.ContainerFromIndex(GuidListBox.SelectedIndex).Focus();
    }
}