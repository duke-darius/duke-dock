using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DukeDock.Models;

public class AppState
{
    public DateTime LastSaved { get; set; }

    public List<StringStoreRecord> StringStoreRecords { get; set; } = new();
    public List<Totp.TotpDefinition> TotpDefinitions { get; set; } = new();
    
    public static void Save(AppState state)
    {
        Directory.CreateDirectory(Utils.ConfigFileDirectory);
        File.WriteAllText(Utils.ConfigFileLocation, JsonConvert.SerializeObject(state));
    }

    public static AppState Load()
    {
        Directory.CreateDirectory(Utils.ConfigFileDirectory);
        return File.Exists(Utils.ConfigFileLocation) ? JsonConvert.DeserializeObject<AppState>(File.ReadAllText(Utils.ConfigFileLocation)) : new AppState();
    }
}