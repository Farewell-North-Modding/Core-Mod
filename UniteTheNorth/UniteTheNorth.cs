using FarewellCore.Tools;
using MelonLoader;
using UniteTheNorth.Systems;
using UniteTheNorth.Tools;
using UnityEngine;

namespace UniteTheNorth;

public class UniteTheNorth : MelonMod
{
    public static MelonLogger.Instance? Logger { get; private set; }
    public const string Version = "0.1.0";
    public static NetPlayer? _netPlayer;

    public override void OnInitializeMelon()
    {
        Logger = LoggerInstance;
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName != "Archipelago") return;
        _netPlayer = PlayerManager.CreateNetPlayer();
    }

    public override void OnFixedUpdate()
    {
        LocalStateSender.SendUpdates();
    }
}