using MessagePack;

namespace UniteTheNorth.Networking.ClientBound;

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
        throw new NotImplementedException();
    }
}