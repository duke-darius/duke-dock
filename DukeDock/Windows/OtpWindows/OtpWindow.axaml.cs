using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using JetBrains.Annotations;
using ReactiveUI;

namespace DukeDock.Windows.OtpWindows;

public partial class OtpWindow : Window
{
    private bool _closeOnDeactivate = true;

    private ReactiveCommand<Unit, Unit> ExitCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> SettingsCommand { [UsedImplicitly] get; }

    public OtpWindow()
    {
        ExitCommand = ReactiveCommand.Create(Close);
        SettingsCommand = ReactiveCommand.CreateFromTask(ShowSettingsWindow);
        InitializeComponent();
        DataContext = this;
    }

    private async Task ShowSettingsWindow()
    {
        _closeOnDeactivate = false;
        var win = new OtpSettingsWindow();
        await win.ShowDialog(this);

        _closeOnDeactivate = true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // ReSharper disable once UnusedParameter.Local
    // ReSharper disable once UnusedParameter.Local
    private void WindowBase_OnDeactivated(object? sender, EventArgs e)
    {
        if(_closeOnDeactivate)
            Close();    
    }
}