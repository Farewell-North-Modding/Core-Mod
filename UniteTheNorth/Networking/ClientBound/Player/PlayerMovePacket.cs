using MessagePack;
using UnityEngine;

namespace UniteTheNorth.Networking.ClientBound.Player;

[MessagePackObject]
public class PlayerMovePacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly Vector3 Position;
    
    public PlayerMovePacket(int id, Vector3 position)
    {
        ID = id;
        Position = position;
    }
    
    public void HandlePacket()
    {
        throw new NotImplementedException();
    }
}