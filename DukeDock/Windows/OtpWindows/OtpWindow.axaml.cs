using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DukeDock.Lib;
using DukeDock.Models.Totp;
using DynamicData;
using JetBrains.Annotations;
using ReactiveUI;

namespace DukeDock.Windows.OtpWindows;

public partial class OtpWindow : FeatureWindow
{
    private ObservableCollection<TotpDefinition> Items { get; set; }
    private ReactiveCommand<Unit, Unit> ExitCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> SettingsCommand { [UsedImplicitly] get; }

    public OtpWindow()
    {
        App.OpenWindows.Add(this);
        ExitCommand = ReactiveCommand.Create(Close);
        SettingsCommand = ReactiveCommand.CreateFromTask(ShowSettingsWindow);
        DataContext = this;

        InitializeComponent();
        
        Redraw();

    }

    private void Redraw()
    {
        Items = new ObservableCollection<TotpDefinition>(App.State.TotpDefinitions);
        
        OtpListBox.Items = Items;
    }

    private async Task ShowSettingsWindow()
    {
        CloseOnDeactivate = false;
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var win = new OtpSettingsWindow();
            await win.ShowDialog(this);

        });
        CloseOnDeactivate = true;
        Redraw();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        OtpListBox = this.FindControl<ListBox>("OtpListBox");
    }



    private void OtpListBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (OtpListBox.Selection.SelectedItem is not TotpDefinition totp ||
            e.Key is not (Key.Space or Key.Enter)) return;
        Application.Current?.Clipboard?.SetTextAsync(totp.GetCode());
        Close();
    }
    
    private void WindowBase_OnActivated(object? sender, EventArgs e)
    {
        OtpListBox.Focus();
    }

    private void TopLevel_OnOpened(object? sender, EventArgs e)
    {
        OtpListBox.ItemContainerGenerator.ContainerFromIndex(OtpListBox.SelectedIndex)?.Focus();

    }

    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        App.OpenWindows.Remove(this);
    }
}