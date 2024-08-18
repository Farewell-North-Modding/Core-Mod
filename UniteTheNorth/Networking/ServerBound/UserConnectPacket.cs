using MessagePack;

namespace UniteTheNorth.Networking.ServerBound;

[MessagePackObject]
public class UserConnectPacket : IServerBoundPacket
{
    [Key(0)] public readonly string Version;
    [Key(1)] public readonly string Username;
    [Key(2)] public readonly string Password;

    public UserConnectPacket(string username, string password)
    {
        Version = UniteTheNorth.Version;
        Username = username;
        Password = password;
    }

    public void HandlePacket(Client client)
    {
        throw new NotImplementedException();
    }
}