namespace Plugin_Namespace;

/// <summary>
/// This class is XML serializing to config file. <br/>
/// Docs: <see href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer?view=netframework-4.7.2"/>
/// </summary>
public partial class Config_Type : IRocketPluginConfiguration
{
    public void LoadDefaults()
    {
        // this method calls when config file creates first time
        // you can set default values for variables here
    }
}