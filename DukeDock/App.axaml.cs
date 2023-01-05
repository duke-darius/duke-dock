using System;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DukeDock.Models;
using DukeDock.Services;
using DukeDock.Windows;
using MonoMac.AppKit;
using MonoMac.Foundation;
using ReactiveUI;
using SharpHook.Native;

namespace DukeDock;

public partial class App : Application
{
    public static AppState State = null!;
    public static MainWindow MainWindow = null!;
    public static ToolWindow? CurrentToolWindow;
    private static readonly KeyBindManager KeyBindManager = new();
    public static readonly string BaseDirectory = AppContext.BaseDirectory;


    public override void Initialize()
    {
        State = AppState.Load();
         
        KeyBindManager.AddHook(new KeyHook(new []{KeyCode.VcLeftShift, KeyCode.VcLeftMeta, KeyCode.VcO}, () =>
        {
            Dispatcher.UIThread.Post(() => {
                if (CurrentToolWindow == null)
                {
                    CurrentToolWindow = new ToolWindow();
                    CurrentToolWindow.Show();
                }
                else
                {
                    CurrentToolWindow.Close();
                    CurrentToolWindow = null;
                }
            });
        }));
        Task.Run(KeyBindManager.RunAsync);

        AvaloniaXamlLoader.Load(this);
        DataContext = this;
    }

    /// <summary>
    /// OSX Helper function used to Hide the dock icon when running as a daemon
    /// </summary>
    public static void HideApplicationInstance()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            NSApplication.SharedApplication.ActivationPolicy = NSApplicationActivationPolicy.Accessory;
        }
    }

    

    public ReactiveCommand<Unit, Unit> OpenCommand { get; } = ReactiveCommand.Create(() =>
    {
        MainWindow.IsVisible = true;
    });

    public override void OnFrameworkInitializationCompleted()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new OsxAppDelegate();
        }
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}

// Sourced: https://github.com/AvaloniaUI/Avalonia/issues/2649#issuecomment-871866559
 public class OsxAppDelegate : NSApplicationDelegate
    {
        public OsxAppDelegate()
        {
            AvaloniaLocator.CurrentMutable.Bind<OsxAppDelegate>().ToConstant(this);
        }

        /// <summary>
        /// Because we are creating our own mac application delegate we are removing / overriding 
        /// the one that Avalonia creates. This causes the application to not be handled as it should.
        /// This is the Avalonia Implementation: https://github.com/AvaloniaUI/Avalonia/blob/5a2ef35dacbce0438b66d9f012e5f629045beb3d/native/Avalonia.Native/src/OSX/app.mm
        /// So what we are doing here is re-creating this implementation to mimic their behavior.
        /// </summary>
        /// <param name="notification"></param>
        public override void WillFinishLaunching(NSNotification notification)
        {
            if (NSApplication.SharedApplication.ActivationPolicy != NSApplicationActivationPolicy.Regular)
            {
                foreach (var x in NSRunningApplication.GetRunningApplications(@"com.apple.dock"))
                {
                    x.Activate(NSApplicationActivationOptions.ActivateIgnoringOtherWindows);
                    break;
                }

                NSApplication.SharedApplication.ActivationPolicy = NSApplicationActivationPolicy.Accessory;
            }
        }

        /// <summary>
        /// Because we are creating our own mac application delegate we are removing / overriding 
        /// the one that Avalonia creates. This causes the application to not be handled as it should.
        /// This is the Avalonia Implementation: https://github.com/AvaloniaUI/Avalonia/blob/5a2ef35dacbce0438b66d9f012e5f629045beb3d/native/Avalonia.Native/src/OSX/app.mm
        /// So what we are doing here is re-creating this implementation to mimic their behavior.
        /// </summary>
        /// <param name="notification"></param>
        public override void DidFinishLaunching(NSNotification notification)
        {
            NSRunningApplication.CurrentApplication.Activate(NSApplicationActivationOptions.ActivateIgnoringOtherWindows);
        }
    }