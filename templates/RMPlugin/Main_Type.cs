using System.Linq.Expressions;

namespace Plugin_Namespace;

public sealed class Main_Type : RocketPlugin<Config_Type>
{
    public static Main_Type Instance { get; private set; }

    protected override void Unload()
    {
    }
    protected override void Load()
    {
        Instance = this;
        Logger.Log(TranslateHello());
    }

    #region Translations
    static char[] disallowedKeyChars = new[] { '_' };
    public new string Translate(string key, params object[] args) => base.Translate(key.Trim(disallowedKeyChars), args);
    /// <summary>
    /// Retrieves values from <see cref="Translations"/> type. [Only "<see langword="public"/> <see langword="static"/> <see langword="readonly"/> <see langword="string"/>" or "<see langword="public"/> <see langword="const"/> <see langword="string"/>" fields]
    /// </summary>
    public override TranslationList DefaultTranslations
    {
        get
        {
            var translations = new TranslationList();
            translations.AddRange(
            typeof(Translations).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(x => x.IsStatic)
            .Select(x => 
                new TranslationListEntry(x.Name.Trim(disallowedKeyChars), x.GetValue(null).ToString())
            ));
            return translations;
        }
    }
    #endregion
}
