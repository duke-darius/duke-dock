using System;
using OtpNet;

namespace DukeDock.Models.Totp;

public class TotpDefinition
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Key { get; set; }
    
    public TotpDefinition(){}

    public TotpDefinition(string name, string key)
    {
        Name = name;
        Key = key;
    }
    
    public string GetCode()
    {
        var otp = new OtpNet.Totp(Base32Encoding.ToBytes(Key));
        return otp.ComputeTotp(DateTime.UtcNow);
    }
}