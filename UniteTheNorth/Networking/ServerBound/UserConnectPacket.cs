using MessagePack;

namespace UniteTheNorth.Networking.ServerBound;

[MessagePackObject]
public class UserConnectPacket
{
    [Key(0)] public readonly string Version;
    [Key(1)] public readonly string Username;
    [Key(2)] public readonly string Password;

    public UserConnectPacket(string version, string username, string password)
    {
        Version = version;
        Username = username;
        Password = password;
    }
}