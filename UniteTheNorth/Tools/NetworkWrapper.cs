using FarewellCore.Tools;
using UniteTheNorth.Networking.Behaviour;

namespace UniteTheNorth.Tools;

public static class NetworkWrapper
{
    public static void CreatePlayerWrapper(int id)
    {
        var player = GameplayFinder.FindPlayer()?.GetGameObject();
        if (player == null) return;
        var animator = player.AddComponent<NetworkAnimator>();
        animator.isHost = true;
        animator.OverwriteSyncId(id);
        var position = player.AddComponent<NetworkPosition>();
        position.isHost = true;
        position.OverwriteSyncId(id);
        var rotation = player.AddComponent<NetworkRotation>();
        rotation.isHost = true;
        rotation.OverwriteSyncId(id);
    }
}