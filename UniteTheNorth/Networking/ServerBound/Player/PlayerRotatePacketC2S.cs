using MessagePack;
using UnityEngine;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerRotatePacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly Quaternion Rotation;
    
    public PlayerRotatePacketC2S(Quaternion rotation)
    {
        Rotation = rotation;
    }

    public void HandlePacket(Server.Client client)
    {
        throw new NotImplementedException();
    }
}