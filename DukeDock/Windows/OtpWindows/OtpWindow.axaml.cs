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
using DukeDock.Controls;
using DukeDock.Models.Totp;
using JetBrains.Annotations;
using ReactiveUI;

namespace DukeDock.Windows.OtpWindows;

public partial class OtpWindow : Window
{
    private bool _closeOnDeactivate = true;

    private ObservableCollection<TotpDefinition> Items { get; set; }
    private ReactiveCommand<Unit, Unit> ExitCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> SettingsCommand { [UsedImplicitly] get; }
    
    

    public OtpWindow()
    {
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
        _closeOnDeactivate = false;
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var win = new OtpSettingsWindow();
            await win.ShowDialog(this);

        });
        _closeOnDeactivate = true;
        Redraw();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        OtpListBox = this.FindControl<ListBox>("OtpListBox");
    }

    // ReSharper disable once UnusedParameter.Local
    // ReSharper disable once UnusedParameter.Local
    private void WindowBase_OnDeactivated(object? sender, EventArgs e)
    {
        if(_closeOnDeactivate)
            Close();    
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
}