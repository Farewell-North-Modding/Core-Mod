using MessagePack;
using UnityEngine;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerMovePacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly Vector3 Position;
    
    public PlayerMovePacketC2S(Vector3 position)
    {
        Position = position;
    }
    
    public void HandlePacket(Server.Client client)
    {
        throw new NotImplementedException();
    }
}