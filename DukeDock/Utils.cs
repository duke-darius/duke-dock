
using System.IO;

namespace DukeDock;

public static class Utils
{

    public static string ConfigFileDirectory => Path.Combine(App.BaseDirectory, "Config");
    public static string ConfigFileLocation => Path.Combine(ConfigFileDirectory, "app.json");
        
}