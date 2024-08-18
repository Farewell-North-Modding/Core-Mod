using MelonLoader;
using UniteTheNorth.Systems;
using UniteTheNorth.Tools;
using FarewellCore;

namespace UniteTheNorth;

public class UniteTheNorth : FarewellMod
{
    public static string Version => BuildInfo.Version;
    public static NetPlayer? _netPlayer;

    public override void OnInitializeMelon()
    {
        
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