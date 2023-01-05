using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace DukeDock.Services;

public class KeyBindManager
{
    private readonly IReactiveGlobalHook _globalHook;
    private readonly List<KeyCode> _codes = new();
    private readonly List<KeyHook> _hooks = new();


    public KeyBindManager()
    {
        _globalHook = new SimpleReactiveGlobalHook();
        _globalHook.KeyPressed.Subscribe(OnKeyPressed);
        _globalHook.KeyReleased.Subscribe(OnKeyReleased);
    }

    public void AddHook(KeyHook hook)
    {
        _hooks.Add(hook);
    }

    public async Task RunAsync()
    {
        await _globalHook.RunAsync();
    }
    
    private void CheckAndExecuteKeyBinds()
    {
        foreach (var hook in _hooks.Where(hook => hook.ShouldExecute(_codes)))
        {
            hook.Execute();
        }
    }

    private void OnKeyPressed(HookEventArgs e)
    {
        if(_codes.Contains(e.RawEvent.Keyboard.KeyCode))
            return;
        _codes.Add(e.RawEvent.Keyboard.KeyCode);
        CheckAndExecuteKeyBinds();
    }

    private void OnKeyReleased(HookEventArgs e)
    {
        if(!_codes.Contains(e.RawEvent.Keyboard.KeyCode))
            return;
        _codes.Remove(e.RawEvent.Keyboard.KeyCode);
    }

}