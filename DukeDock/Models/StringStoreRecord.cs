using System;

namespace DukeDock.Models;

public class StringStoreRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Value { get; set; }
    public string? Name { get; set; }
}