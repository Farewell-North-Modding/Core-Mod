using LiteNetLib;
using MessagePack;
using UniteTheNorth.Networking.ClientBound.Player;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerAnimatePacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly int PropertyHash;
    [Key(1)] public readonly int Type;
    [Key(2)] public readonly object Value;

    public PlayerAnimatePacketC2S(int propertyHash, object value, int type)
    {
        PropertyHash = propertyHash;
        Value = value;
        Type = type;
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(new PlayerAnimatePacket(
            client.ID,
            PropertyHash,
            Value,
            Type
        ), DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}