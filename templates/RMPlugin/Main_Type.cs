using System.Linq.Expressions;

namespace Plugin_Namespace;

public sealed partial class Main_Type : RocketPlugin<Config_Type>
{
    protected override void Unload()
    {
    }
    protected override void Load()
    {
        Logger.Log(TranslateHello());
    }
}