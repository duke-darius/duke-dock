using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DukeDock.Models.Totp;
using ReactiveUI.Fody.Helpers;

namespace DukeDock.Controls;

public partial class OtpDisplay : UserControl
{
    public DispatcherTimer timer { get; set; }
    public bool IsHigh { get; set; } = DateTime.Now.Second >= 30;
    private bool currentIsHigh => DateTime.Now.Second >= 30;
    [Reactive] private TotpDefinition Def { get; set; }
    public string Code { get; set; } = "";

    [Reactive] public string Time { get; set; } = "30";


    public static readonly DirectProperty<OtpDisplay, TotpDefinition> IdProperty =
        AvaloniaProperty.RegisterDirect<OtpDisplay, TotpDefinition>(nameof(Id), o => o.Id,
            (o, v) => o.Id = v);

    [Reactive]
    public TotpDefinition Id
    {
        get => _id;
        set
        {
            Console.WriteLine(value);
            _id = value;
            Def = value;
            
        }
    }

    private TotpDefinition _id;



    public OtpDisplay()
    {
        InitializeComponent();
        Remaining.DataContext = this;
        TimeTextBox.DataContext = this;
        RefreshCode();
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(100);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    
    private void Timer_Tick(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            
            if(currentIsHigh != IsHigh)
            {
                RefreshCode();
            }
            var seconds = DateTime.Now.Second + (DateTime.Now.Millisecond / 1000d);
            if (currentIsHigh)
                seconds -= 30;
            Remaining.Value = seconds;
            TimeTextBox.Text = (30 - seconds).ToString("00");
            
        });
    }

    public string DefName { get; set; } 
    private void RefreshCode()
    {
        Dispatcher.UIThread.Post(() =>
        {
            Code = Def?.GetCode();
            CodeTextBox.Text = Code;
            IsHigh = currentIsHigh;
        }, DispatcherPriority.Render);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        CodeTextBox = this.FindControl<TextBlock>("CodeTextBox");
        NameTextBox = this.FindControl<TextBlock>("NameTextBox");
        Remaining = this.FindControl<ProgressBar>("Remaining");
        TimeTextBox = this.FindControl<TextBlock>("TimeTextBox");
        
        Remaining.Maximum = 30;
        Remaining.Minimum = 0;
        Remaining.Transitions = null;
    }
}