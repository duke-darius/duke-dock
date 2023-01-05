using System;
using System.Collections.Generic;
using System.Linq;
using SharpHook.Native;

namespace DukeDock.Services;

public class KeyHook
{
    private readonly IEnumerable<KeyCode> _keyBind;
    private readonly Action _onExecute;

    public KeyHook(IEnumerable<KeyCode> keyBind, Action onExecute)
    {
        _keyBind = keyBind;
        _onExecute = onExecute;
    }

    public bool ShouldExecute(IEnumerable<KeyCode> currentCodes) => _keyBind.All(currentCodes.Contains);

    public void Execute()
    {
        _onExecute.Invoke();
    }
}