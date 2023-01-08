using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DukeDock.Lib;

public partial class FeatureWindow : Window
{
    public bool CloseOnDeactivate = true;
    
    public FeatureWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    // ReSharper disable once UnusedParameter.Local
    // ReSharper disable once UnusedParameter.Local
    private void WindowBase_OnDeactivated(object? sender, EventArgs e)
    {
        if(CloseOnDeactivate)
            Close();    
    }
}