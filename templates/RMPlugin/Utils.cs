﻿global using Rocket.API.Collections;
global using Rocket.API;
global using Rocket.API.Serialisation;
global using Rocket.Core;
global using Rocket.Core.Assets;
global using Rocket.Core.Logging;
global using Rocket.Core.Plugins;
global using Rocket.Core.Utils;
global using Rocket.Unturned.Chat;
global using Rocket.Unturned.Events;
global using Rocket.Unturned;
global using Rocket.Unturned.Player;
global using SDG.Unturned;
global using SDG.NetPak;
global using SDG.NetTransport;
global using SDG.Framework.Landscapes;
global using SDG.Framework.Utilities;
global using System;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Collections;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Xml.Serialization;
global using Steamworks;
global using UnityEngine;
global using Color = UnityEngine.Color;
global using UnityComponent = UnityEngine.Component;
global using UnityObject = UnityEngine.Object;
global using UnityBehaviour = UnityEngine.MonoBehaviour;
global using Logger = Rocket.Core.Logging.Logger;
global using UP = Rocket.Unturned.Player.UnturnedPlayer;
global using UPlayer = Rocket.Unturned.Player.UnturnedPlayer;
global using IRP = Rocket.API.IRocketPlayer;
global using IRPlayer = Rocket.API.IRocketPlayer;
global using SP = SDG.Unturned.SteamPlayer;
global using SPlayer = SDG.Unturned.SteamPlayer;
global using P = SDG.Unturned.Player;
global using Player = SDG.Unturned.Player;
global using Vehicle = SDG.Unturned.InteractableVehicle;
global using Storage = SDG.Unturned.InteractableStorage;
global using InventoryPage = SDG.Unturned.Items;
global using static Plugin_Namespace.Utils;

namespace Plugin_Namespace;

public static partial class Utils
{
    // just shorthands, you can simply remove/rename them if you want to.
    public static Main_Type inst => Main_Type.Instance;
    public static Config_Type conf => inst.Configuration.Instance;
    //
}
