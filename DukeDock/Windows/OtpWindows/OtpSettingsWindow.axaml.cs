using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DukeDock.Models;
using DukeDock.Models.Totp;
using DynamicData;
using JetBrains.Annotations;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DukeDock.Windows.OtpWindows;

public partial class OtpSettingsWindow : Window
{
    [Reactive] private TotpDefinition? SelectedOtp { get; set; }
    private ObservableCollection<TotpDefinition> Otps { [UsedImplicitly] get; }
    private readonly AppState _tempAppState;

    private ReactiveCommand<Unit, Unit> AddCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> RemoveCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> SaveCommand { [UsedImplicitly] get; }
    private ReactiveCommand<Unit, Unit> CancelCommand { [UsedImplicitly] get; }
    public OtpSettingsWindow()
    {
        App.OpenWindows.Add(this);
        // Deep copy
        _tempAppState = JsonConvert.DeserializeObject<AppState>(JsonConvert.SerializeObject(App.State));
        Otps = new ObservableCollection<TotpDefinition>(_tempAppState.TotpDefinitions);
        AddCommand = ReactiveCommand.Create(() =>
        {
            Otps.Add(new TotpDefinition("New Otp Definition", ""));
        });
        RemoveCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedOtp != null)
                Otps.Remove(SelectedOtp);
        });
        SaveCommand = ReactiveCommand.Create(SaveAndClose);
        CancelCommand = ReactiveCommand.Create(Close);

        InitializeComponent();
        DataContext = this;
    }

    private void SaveAndClose()
    {
        if (SelectedOtp != null)
        {
            SelectedOtp.Key = KeyTextBox.Text;
            SelectedOtp.Name = NameTextBox.Text;
        }
        _tempAppState.TotpDefinitions = Otps.ToList();
        App.State.TotpDefinitions = _tempAppState.TotpDefinitions;
        AppState.Save(App.State);
        Close();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        OtpListBox = this.FindControl<ListBox>("OtpListBox");
        EditStackPanel = this.FindControl<StackPanel>("EditStackPanel");
        NameTextBox = this.FindControl<TextBox>("NameTextBox");
        KeyTextBox = this.FindControl<TextBox>("KeyTextBox");
    }

    private void OtpListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            if (SelectedOtp != null)
            {
                SelectedOtp.Key = KeyTextBox.Text;
                SelectedOtp.Name = NameTextBox.Text;
            }

            SelectedOtp = OtpListBox?.Selection?.SelectedItem as TotpDefinition;
        
            EditStackPanel.IsVisible = SelectedOtp != null;

            if (SelectedOtp == null) return;
            
            NameTextBox.Text = SelectedOtp.Name;
            KeyTextBox.Text = SelectedOtp.Key;
        });
    }

    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        App.OpenWindows.Remove(this);
    }
}