using LiteNetLib;
using MessagePack;
using UniteTheNorth.Networking.ClientBound.Player;

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
        PacketManager.SendToAll(new PlayerAnimatePacket(
            client.ID,
            PropertyHash,
            Value
        ), DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}