using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DukeDock.Models;
using JetBrains.Annotations;
using ReactiveUI;

namespace DukeDock.Windows;

public partial class StringStoreWindow : Window
{
    private StringStoreRecord? _selectedRecord = null;
    private ObservableCollection<StringStoreRecord> Records { get; set; } = new(App.State.StringStoreRecords);
    private bool _closeOnDeactivate = true;
    public StringStoreWindow()
    {

        AddRecordCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            try
            {
                _closeOnDeactivate = false;
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {

                    var win = new StringStoreAddRecordWindow();
                    var res = await win.ShowDialog<StringStoreAddRecordWindow.StringStoreAddRecordViewModel?>(this);

                    if (res != null)
                    {
                        App.State.StringStoreRecords.Add(new StringStoreRecord
                        {
                            Name = res.Name,
                            Value = res.Value
                        });
                        AppState.Save(App.State);
                        Redraw();
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _closeOnDeactivate = true;
        });
        CopyRecordCommand = ReactiveCommand.Create(() =>
        {
            Application.Current?.Clipboard?.SetTextAsync(_selectedRecord?.Value ?? "");
            Close();
        });
        CancelCommand = ReactiveCommand.Create(Close);
        
        InitializeComponent();

        DataContext = this;
        TextListBox.Focus();

        Redraw();

    }

    private void Redraw()
    {
        Records = new ObservableCollection<StringStoreRecord>(App.State.StringStoreRecords);
        TextListBox.Items = Records;
    }

    private ReactiveCommand<Unit, Unit> AddRecordCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> CopyRecordCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> CancelCommand { [UsedImplicitly] get; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        TextListBox = this.FindControl<ListBox>("TextListBox");
    }

    private void TextListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedRecord = TextListBox?.Selection?.SelectedItem as StringStoreRecord;
    }

    private void TopLevel_OnOpened(object? sender, EventArgs e)
    {
        TextListBox.ItemContainerGenerator.ContainerFromIndex(TextListBox.SelectedIndex).Focus();
    }

    private void WindowBase_OnDeactivated(object? sender, EventArgs e)
    {
        if(_closeOnDeactivate)
            Dispatcher.UIThread.Post(Close);
    }
}