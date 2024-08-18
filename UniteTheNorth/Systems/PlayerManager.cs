using FarewellCore.Tools;
using UniteTheNorth.Tools;

namespace UniteTheNorth.Systems;

public static class PlayerManager
{
    public static NetPlayer? CreateNetPlayer()
    {
        var newObject = GameplayFinder.FindPlayer()?.GetGameObject();
        return newObject?.AddComponent<NetPlayer>();
    }
}