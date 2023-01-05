using System;
using System.ComponentModel;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DukeDock.Windows.OtpWindows;
using ReactiveUI;

namespace DukeDock.Windows;

public partial class ToolWindow : Window
{
    public ToolWindow() : this(null)
    {
        
    }
    public ToolWindow(PixelPoint? position = null)
    {
        InitializeComponent();
        DataContext = this;
        if (position != null)
        {
            Position = position.Value;
            Console.WriteLine(position);
        }
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
}