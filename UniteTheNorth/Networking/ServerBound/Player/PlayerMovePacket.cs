using MessagePack;
using UnityEngine;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerMovePacket : IServerBoundPacket
{
    [Key(0)] public readonly Vector3 Position;
    
    public PlayerMovePacket(Vector3 position)
    {
        Position = position;
    }
    
    public void HandlePacket(Client client)
    {
        throw new NotImplementedException();
    }
}