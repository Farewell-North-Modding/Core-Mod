using MelonLoader;
using UniteTheNorth.Systems;
using FarewellCore;

namespace UniteTheNorth;

public class UniteTheNorth : FarewellMod
{
    public static string Version => BuildInfo.Version;
    public static MelonLogger.Instance Logger => Melon<UniteTheNorth>.Logger;

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName != "Archipelago") return;
        // TODO: Call Player Manager
    }

    public override void OnFixedUpdate()
    {
        LocalStateSender.SendUpdates();
    }
}