using MessagePack;
using UnityEngine;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerRotatePacket : IServerBoundPacket
{
    [Key(0)] public readonly Quaternion Rotation;
    
    public PlayerRotatePacket(Quaternion rotation)
    {
        Rotation = rotation;
    }

    public void HandlePacket(Client client)
    {
        throw new NotImplementedException();
    }
}