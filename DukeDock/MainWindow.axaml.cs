using System;
using System.ComponentModel;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DukeDock.Windows;
using ReactiveUI;

namespace DukeDock;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        
        App.MainWindow = this;
        InitializeComponent();
        DataContext = this;

    }

    public ReactiveCommand<Unit, Unit> OtpClicked { get; } = ReactiveCommand.Create(() =>
    {
        Console.WriteLine("Otp Clicked");
    });

    public ReactiveCommand<Unit,Unit> StringStoreClicked { get; } = ReactiveCommand.Create(() =>
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
        e.Cancel = true;
        Hide();
    }

    private void WindowBase_OnDeactivated(object? sender, EventArgs e)
    {
        Hide();
    }
}