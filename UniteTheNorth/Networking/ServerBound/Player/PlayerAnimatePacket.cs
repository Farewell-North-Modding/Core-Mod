using MessagePack;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerAnimatePacket : IServerBoundPacket
{
    [Key(0)] public readonly int PropertyHash;
    [Key(1)] public readonly object Value;

    public PlayerAnimatePacket(int propertyHash, object value)
    {
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket(Client client)
    {
        throw new NotImplementedException();
    }
}