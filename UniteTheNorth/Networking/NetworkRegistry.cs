using UniteTheNorth.Networking.Behaviour;

namespace UniteTheNorth.Networking;

public static class NetworkRegistry
{
    private static readonly Dictionary<int, NetworkAnimator> Animators = new();
    private static readonly Dictionary<int, NetworkPosition> Positions = new();
    private static readonly Dictionary<int, NetworkRotation> Rotations = new();

    public static void RegisterAnimator(NetworkAnimator networkAnimator)
    {
        Animators[networkAnimator.GetSyncId()] = networkAnimator;
    }

    public static void RegisterPosition(NetworkPosition networkPosition)
    {
        Positions[networkPosition.GetSyncId()] = networkPosition;
    }

    public static void RegisterRotation(NetworkRotation networkRotation)
    {
        Rotations[networkRotation.GetSyncId()] = networkRotation;
    }

    public static NetworkAnimator? GetNetworkAnimator(int id)
    {
        return Animators.GetValueOrDefault(id);
    }

    public static NetworkPosition? GetNetworkPosition(int id)
    {
        return Positions.GetValueOrDefault(id);
    }
    
    public static NetworkRotation? GetNetworkRotation(int id)
    {
        return Rotations.GetValueOrDefault(id);
    }
}