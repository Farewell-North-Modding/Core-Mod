using LiteNetLib;
using MessagePack;
using UniteTheNorth.Networking.ClientBound.Player;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerAnimateBoolPacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly int PropertyHash;
    [Key(1)] public readonly bool Value;

    public PlayerAnimateBoolPacketC2S(int propertyHash, bool value)
    {
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(new PlayerAnimateBoolPacket(
            client.ID,
            PropertyHash,
            Value
        ), DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}

[MessagePackObject]
public class PlayerAnimateFloatPacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly int PropertyHash;
    [Key(1)] public readonly float Value;

    public PlayerAnimateFloatPacketC2S(int propertyHash, float value)
    {
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(new PlayerAnimateFloatPacket(
            client.ID,
            PropertyHash,
            Value
        ), DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}

[MessagePackObject]
public class PlayerAnimateIntPacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly int PropertyHash;
    [Key(1)] public readonly int Value;

    public PlayerAnimateIntPacketC2S(int propertyHash, int value)
    {
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(new PlayerAnimateIntPacket(
            client.ID,
            PropertyHash,
            Value
        ), DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}