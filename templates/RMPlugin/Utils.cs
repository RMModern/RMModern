global using Rocket.API.Collections;
global using Rocket.API;
global using Rocket.Core.Logging;
global using Rocket.Core.Plugins;
global using Rocket.Unturned.Chat;
global using Rocket.Unturned.Events;
global using Rocket.Unturned;
global using Rocket.Unturned.Player;
global using SDG.Unturned;
global using System;
global using System.Threading;
global using System.IO;
global using System.Collections.Generic;
global using UnityEngine;
global using System.Linq;
global using System.Collections;
global using System.Text;
global using System.Text.RegularExpressions;
global using Steamworks;
global using Logger = Rocket.Core.Logging.Logger;
global using Rocket.API.Serialisation;
global using Rocket.Core;
global using Color = UnityEngine.Color;
global using UP = Rocket.Unturned.Player.UnturnedPlayer;
global using IRP = Rocket.API.IRocketPlayer;
global using SP = SDG.Unturned.SteamPlayer;
global using P = SDG.Unturned.Player;
global using V = SDG.Unturned.InteractableVehicle;
global using System.Reflection;
global using System.Xml.Serialization;
global using Rocket.Core.Assets;
global using static Plugin_Namespace.Utils;

namespace Plugin_Namespace;

public static partial class Utils
{
    public static Main_Type inst => Main_Type.Instance;
    public static Config_Type conf => inst.Configuration.Instance;
}
