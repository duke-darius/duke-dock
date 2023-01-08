using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DukeDock.Lib;
using Brush = Avalonia.Media.Brush;
using Brushes = Avalonia.Media.Brushes;

namespace DukeDock.Controls;

public partial class SearchResult : UserControl
{
    public Feature? Feature { get; }
    public string? FeatureName => Feature?.FeatureName;
    private readonly IBrush _selectedColor = new SolidColorBrush(Colors.Snow, 0.2);
    private bool _isActive { get; set; }


    public SearchResult(){}
    public SearchResult(Feature? feature)
    {
        Feature = feature;
        InitializeComponent();
        DataContext = this;
    }

    public bool GetActive()
    {
        return _isActive;
    }
    public void SetActive(bool active)
    {
        _isActive = active;
        Background = _isActive ? _selectedColor : Brushes.Transparent;
    }
    
}