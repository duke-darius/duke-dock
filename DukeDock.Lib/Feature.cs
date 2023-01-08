using System;

namespace DukeDock.Lib;

public class Feature
{
    public string FeatureName { get; set; }
    public Type WindowType { get; set; }


    public Feature(string name, Type type)
    {
        FeatureName = name;
        WindowType = type;
    }
}