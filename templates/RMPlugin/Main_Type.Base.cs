﻿namespace Plugin_Namespace;

// this base handling shutdown and multiple instances of this component
partial class Main_Type
{
    private Main_Type() { }
    public override void LoadPlugin()
    {
        AssertInstance(false);
        Instance = this;
        base.LoadPlugin();
        StartHandlingShutdown();
    }
    public override void UnloadPlugin(PluginState state = PluginState.Unloaded)
    {
        base.UnloadPlugin(state);
        Instance = null;
    }

    private static bool InstanceExists() => Instance is not null;

    private static void AssertInstance(bool exists)
    {
        if (InstanceExists() != exists)
            throw new Exception($"{typeof(Main_Type).FullName} instance already {(exists ? "not exists" : $"exists, use {nameof(Main_Type)}.{nameof(Instance)} to access it's instance")}.");
    }
    public static Main_Type Instance { get; private set; }

    private void StopHandlingShutdown()
    {
        Application.quitting -= OnShutdown;
        Provider.onCommenceShutdown -= OnShutdown;
    }

    private void StartHandlingShutdown()
    {
        StopHandlingShutdown();
        Application.quitting += OnShutdown;
        Provider.onCommenceShutdown += OnShutdown;
    }

    private void OnShutdown()
    {
        if (!InstanceExists())
            return;
        StopHandlingShutdown();
        UnloadPlugin();
    }

    private void OnApplicationQuit() => OnShutdown();
    private void OnDestroy() => OnShutdown();
}