using MessagePack;
using UniteTheNorth.Systems;

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
        switch (Value)
        {
            case bool boolVal:
                PlayerManager.RunOnPlayer(ID, player => player.ReceiveAnimationBool(PropertyHash, boolVal));
                break;
            case float floatVal:
                PlayerManager.RunOnPlayer(ID, player => player.ReceiveAnimationFloat(PropertyHash, floatVal));
                break;
            case int intVal:
                PlayerManager.RunOnPlayer(ID, player => player.ReceiveAnimationInt(PropertyHash, intVal));
                break;
            default:
                UniteTheNorth.Logger.Warning($"[Client] Received invalid animation value: {Value.GetType()}");
                break;
        }
    }
}