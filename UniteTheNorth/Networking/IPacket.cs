namespace UniteTheNorth.Networking;

public interface IClientBoundPacket
{
    public void HandlePacket();
}

public interface IServerBoundPacket
{
    public void HandlePacket(Client client);
}

public interface IBiDirectionalPacket : IClientBoundPacket, IServerBoundPacket {  }