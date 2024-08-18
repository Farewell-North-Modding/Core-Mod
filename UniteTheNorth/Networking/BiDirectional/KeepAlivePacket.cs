using MessagePack;

namespace UniteTheNorth.Networking.BiDirectional;

[MessagePackObject]
public class KeepAlivePacket : IBiDirectionalPacket
{
    public void HandlePacket()
    {
        throw new NotImplementedException();
    }

    public void HandlePacket(Client client)
    {
        throw new NotImplementedException();
    }
}