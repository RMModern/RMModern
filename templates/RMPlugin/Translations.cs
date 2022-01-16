global using static Plugin_Namespace.Translations;

namespace Plugin_Namespace;

/// <summary>
/// Don't rename this class, it will be used in analyzer.
/// </summary>
public static partial class Translations
{
    #region Base
    /// <summary>
    /// This method is important for analyzer!
    /// </summary>
    public static string Translate(string translationKey, params object[] arguments) => inst.Translate(translationKey, arguments);
    #endregion
    // You can write translations here. By default _ will be trimmed
    public const string 
        Hello = "Hello from RocketMod.Modern!";
}
