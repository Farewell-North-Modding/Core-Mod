using MessagePack;
using UniteTheNorth.Tools;

namespace UniteTheNorth.Networking.ClientBound;

[MessagePackObject]
public class UserDataPacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;

    public UserDataPacket(int id)
    {
        ID = id;
    }

    public void HandlePacket()
    {
        UniteTheNorth.Logger.Msg($"[Client] Received local id ({ID})");
        NetworkWrapper.CreatePlayerWrapper(ID);
    }
}