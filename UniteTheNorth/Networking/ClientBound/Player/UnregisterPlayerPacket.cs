using MessagePack;
using UniteTheNorth.Systems;

namespace UniteTheNorth.Networking.ClientBound.Player;

[MessagePackObject]
public class UnregisterPlayerPacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;

    public UnregisterPlayerPacket(int id)
    {
        ID = id;
    }

    public void HandlePacket()
    {
        PlayerManager.UnregisterPlayer(ID);
    }
}