using MessagePack;
using UniteTheNorth.Systems;

namespace UniteTheNorth.Networking.ClientBound.Player;

[MessagePackObject]
public class PlayerAnimatePacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly int PropertyHash;
    [Key(2)] public readonly int Type;
    [Key(3)] public readonly object Value;

    public PlayerAnimatePacket(int id, int propertyHash, int type, object value)
    {
        ID = id;
        PropertyHash = propertyHash;
        Type = type;
        Value = value;
    }

    public void HandlePacket()
    {
        switch (Type)
        {
            case 0:
                PlayerManager.RunOnPlayer(ID, player => player.ReceiveAnimationBool(PropertyHash, (bool) Value));
                break;
            case 1:
                PlayerManager.RunOnPlayer(ID, player => player.ReceiveAnimationFloat(PropertyHash, (float) Value));
                break;
            case 2:
                PlayerManager.RunOnPlayer(ID, player => player.ReceiveAnimationInt(PropertyHash, (int) Value));
                break;
            default:
                UniteTheNorth.Logger.Warning($"[Client] Received invalid animation value type: {Type}");
                break;
        }
    }
}