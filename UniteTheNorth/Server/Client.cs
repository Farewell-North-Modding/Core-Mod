using LiteNetLib;

namespace UniteTheNorth.Server;

public class Client
{
    public readonly int ID;
    public readonly string Username;
    public readonly NetPeer Peer;
    
    public Client(NetPeer peer, string username, int id)
    {
        Peer = peer;
        Username = username;
        ID = id;
    }
}