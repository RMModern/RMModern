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
    public override TranslationList DefaultTranslations => DefaultTranslationList;

    public new string Translate(string key, params object[] args) => base.Translate(key.Trim(TranslationKeyTrimCharacters), args);
    #endregion
}