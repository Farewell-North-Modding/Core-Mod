using MessagePack;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerAnimatePacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly int PropertyHash;
    [Key(1)] public readonly object Value;

    public PlayerAnimatePacketC2S(int propertyHash, object value)
    {
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket(Server.Client client)
    {
        throw new NotImplementedException();
    }
}