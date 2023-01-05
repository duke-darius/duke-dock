using System;
using System.ComponentModel;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DukeDock.Windows;
using DukeDock.Windows.OtpWindows;
using ReactiveUI;

namespace DukeDock;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        App.MainWindow = this;
        InitializeComponent();

        Opened += (sender, args) => Close();
    }

}