using MessagePack;
using UniteTheNorth.Systems;

namespace UniteTheNorth.Networking.ClientBound.Player;

[MessagePackObject]
public class RegisterPlayerPacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly string Username;

    public RegisterPlayerPacket(int id, string username)
    {
        ID = id;
        Username = username;
    }

    public void HandlePacket()
    {
        PlayerManager.RegisterPlayer(ID, Username);
    }
}