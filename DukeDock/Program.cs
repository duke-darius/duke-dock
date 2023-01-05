using Avalonia;
using System;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;

namespace DukeDock;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        BuildAvaloniaApp()
            .UseReactiveUI()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}