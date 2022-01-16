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
    static char[] translationKeyTrimCharacters = new[] { '_' };
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
            .Where(x => x.IsStatic || x.IsLiteral)
            .Select(x =>
                new TranslationListEntry(x.Name.Trim(translationKeyTrimCharacters), (x.IsLiteral ? x.GetRawConstantValue() : x.GetValue(null)).ToString())
            ));
            return translations;
        }
    }
    public new string Translate(string key, params object[] args) => base.Translate(key.Trim(translationKeyTrimCharacters), args);
    #endregion
}