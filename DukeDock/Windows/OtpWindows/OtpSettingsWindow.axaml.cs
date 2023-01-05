using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DukeDock.Windows.OtpWindows;

public partial class OtpSettingsWindow : Window
{
    public OtpSettingsWindow()
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
}