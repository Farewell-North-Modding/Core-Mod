using MessagePack;

namespace UniteTheNorth.Networking.ClientBound.Player;

[MessagePackObject]
public class PlayerAnimatePacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly object Value;

    public PlayerAnimatePacket(int id, int propertyHash, object value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket()
    {
        throw new NotImplementedException();
    }
}