using System;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace DukeDock.Windows;

public partial class StringStoreAddRecordWindow : Window
{
    private StringStoreAddRecordViewModel Model { get; } = new();

    private ReactiveCommand<Unit, Unit> CancelCommand { get; }
    private ReactiveCommand<Unit, Unit> SaveCommand { get; }
    
    public StringStoreAddRecordWindow()
    {
        App.OpenWindows.Add(this);
        DataContext = this;
        CancelCommand = ReactiveCommand.Create(() =>
        {
            Close(null);
        });
        SaveCommand = ReactiveCommand.Create(() =>
        {
            Close(Model);
        });
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public class StringStoreAddRecordViewModel : ReactiveObject
    {
        private string? _name;
        private string? _value;

        public string? Name
        {
            get => _name;
            set => _name = this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string? Value
        {
            get => _value;
            set => _value = this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        App.OpenWindows.Remove(this);
    }
}