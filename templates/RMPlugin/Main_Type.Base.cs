namespace Plugin_Namespace;

// this base handling shutdown and multiple instances of this component
partial class Main_Type
{
    Main_Type() { }
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
    static bool InstanceExists() => Instance is not null;
    static void AssertInstance(bool exists)
    {
        if (InstanceExists() != exists)
            throw new Exception($"{typeof(Main_Type).FullName} instance already exists, use {nameof(Main_Type)}.{nameof(Instance)} to access it's instance.");
    }
    public static Main_Type Instance { get; private set; }
    void StopHandlingShutdown()
    {
        Application.quitting -= OnShutdown;
        Provider.onCommenceShutdown -= OnShutdown;
    }
    void StartHandlingShutdown()
    {
        StopHandlingShutdown();
        Application.quitting += OnShutdown;
        Provider.onCommenceShutdown += OnShutdown;
    }
    void OnShutdown()
    {
        AssertInstance(true);
        Instance = null;
        StopHandlingShutdown();
        UnloadPlugin();
    }
    void OnApplicationQuit() => OnShutdown();
    void OnDestroy() => OnShutdown();
}