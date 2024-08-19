using LiteNetLib;
using MessagePack;

namespace UniteTheNorth.Networking.BiDirectional.Generic;

[MessagePackObject]
public class AnimateBoolPacket : IBiDirectionalPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly bool Value;

    public AnimateBoolPacket(int id, int propertyHash, bool value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket()
    {
        NetworkRegistry.GetNetworkAnimator(ID)?.ReceiveAnimationBool(PropertyHash, Value);
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(this, DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}

[MessagePackObject]
public class AnimateFloatPacket : IBiDirectionalPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly float Value;

    public AnimateFloatPacket(int id, int propertyHash, float value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket()
    {
        NetworkRegistry.GetNetworkAnimator(ID)?.ReceiveAnimationFloat(PropertyHash, Value);
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(this, DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}

[MessagePackObject]
public class AnimateIntPacket : IBiDirectionalPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly int Value;

    public AnimateIntPacket(int id, int propertyHash, int value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket()
    {
        NetworkRegistry.GetNetworkAnimator(ID)?.ReceiveAnimationInt(PropertyHash, Value);
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(this, DeliveryMethod.Unreliable, Channels.Medium, client);
    }
}