using MessagePack;

namespace UniteTheNorth.Networking.ServerBound;

[MessagePackObject]
public class UserInformationPacket : IServerBoundPacket
{
    [Key(0)] public readonly string Version;
    [Key(1)] public readonly string Username;

    public UserInformationPacket(string username)
    {
        Version = UniteTheNorth.Version;
        Username = username;
    }

    public void HandlePacket(Client client)
    {
        throw new NotImplementedException();
    }
}