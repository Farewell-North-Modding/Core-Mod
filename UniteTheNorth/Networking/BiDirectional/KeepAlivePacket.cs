using LiteNetLib;
using MessagePack;

namespace UniteTheNorth.Networking.BiDirectional;

[MessagePackObject]
public class KeepAlivePacket : IBiDirectionalPacket
{
    public void HandlePacket()
    {
        PacketManager.Send(this, DeliveryMethod.Unreliable, Channels.Unimportant);
    }

    public void HandlePacket(Server.Client client)
    {
        // Ignore for now
    }
}