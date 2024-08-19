using MessagePack;
using UniteTheNorth.Systems;

namespace UniteTheNorth.Networking.ClientBound.Player;

[MessagePackObject]
public class PlayerAnimateBoolPacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly bool Value;

    public PlayerAnimateBoolPacket(int id, int propertyHash, bool value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket()
    {
        PlayerManager.RunOnPlayer(ID, player => 
            player.ReceiveAnimationBool(PropertyHash, Value));
    }
}

[MessagePackObject]
public class PlayerAnimateFloatPacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly float Value;

    public PlayerAnimateFloatPacket(int id, int propertyHash, float value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket()
    {
        PlayerManager.RunOnPlayer(ID, player => 
            player.ReceiveAnimationFloat(PropertyHash, Value));
    }
}

[MessagePackObject]
public class PlayerAnimateIntPacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly int Value;

    public PlayerAnimateIntPacket(int id, int propertyHash, int value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Value = value;
    }

    public void HandlePacket()
    {
        PlayerManager.RunOnPlayer(ID, player => 
            player.ReceiveAnimationInt(PropertyHash, Value));
    }
}