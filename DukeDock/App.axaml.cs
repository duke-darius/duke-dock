using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DukeDock.Models;
using DukeDock.Services;
using ReactiveUI;
using SharpHook.Native;

namespace DukeDock;

public partial class App : Application
{
    public static AppState State = null!;
    public static MainWindow MainWindow = null!;
    private static readonly KeyBindManager KeyBindManager = new();
    public static readonly string BaseDirectory = AppContext.BaseDirectory;


    public override void Initialize()
    {
        State = AppState.Load();
         
        KeyBindManager.AddHook(new KeyHook(new []{KeyCode.VcLeftShift, KeyCode.VcLeftMeta, KeyCode.VcO}, () =>
        {
            MainWindow.ToggleVisibility();
        }));
        Task.Run(KeyBindManager.RunAsync);

        AvaloniaXamlLoader.Load(this);
        DataContext = this;
    }



    public ReactiveCommand<Unit, Unit> OpenCommand { get; } = ReactiveCommand.Create(() =>
    {
        MainWindow.IsVisible = true;
    });

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}