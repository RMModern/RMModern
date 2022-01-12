global using static Plugin_Namespace.Translations;

namespace Plugin_Namespace;

/// <summary>
/// Don't rename this class, it will be used in analyzer.
/// </summary>
public static partial class Translations
{
    /// <summary>
    /// This method is important for analyzer!
    /// </summary>
    public static string Translate(string translationKey, params object[] arguments) => inst.Translate(translationKey, arguments);
    // You can write translations here. By default _ will be trimmed
    public static readonly string
        Hello = "Hello from RocketMod.Modern!";
}
