using System.Net;
using System.Net.Sockets;
using System.Text;
using LiteNetLib;
using MessagePack;
using UniteTheNorth.Networking;
using UniteTheNorth.Networking.ClientBound;
using UniteTheNorth.Networking.ClientBound.Player;
using UniteTheNorth.Networking.ServerBound;
using UnityEngine;

namespace UniteTheNorth.Server;

public class Server : MonoBehaviour, INetEventListener
{
    private const int Port = 4657;
    private const string? Password = null;
    public static Server? Instance;
    
    public NetManager? NetServer;
    public readonly Dictionary<int, Client> Clients = new();
    
    private void Start()
    {
        Instance = this;
        NetServer = new NetManager(this)
        {
            UpdateTime = 15
        };
        NetServer.Start(Port);
    }

    private void Update()
    {
        NetServer?.PollEvents();
    }

    private void OnDestroy()
    {
        NetServer?.Stop();
    }

    private bool TryGetClient(NetPeer peer, out Client? client)
    {
        client = Clients.Values.FirstOrDefault(client => Equals(client.Peer, peer));
        return client != null;
    }

    public void OnPeerConnected(NetPeer peer)
    {
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        if (TryGetClient(peer, out var client))
        {
            Clients.Remove(client!.ID);
            UniteTheNorth.Logger.Msg($"[Server] User {client!.Username} disconnected: {disconnectInfo.Reason}");
            PacketManager.SendToAll(new UnregisterPlayerPacket(
                client.ID    
            ), DeliveryMethod.ReliableOrdered, Channels.System);
        }
        else
            UniteTheNorth.Logger.Msg($"[Server] Connection from {peer.Address} disconnected: {disconnectInfo.Reason}");
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        UniteTheNorth.Logger.Warning($"[Server] There was a socket error ({socketError}) on {endPoint}");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        if(TryGetClient(peer, out var client))
            PacketManager.HandlePacket(client!, reader);
        else
            UniteTheNorth.Logger.Warning($"[Server] Received data from unknown peer: {peer.Address}");
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        // Do absolutely nothing :D
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        // Do absolutely nothing again :D
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        // Validate Connection
        UniteTheNorth.Logger.Msg($"[Server] New Connection from {request.RemoteEndPoint}");
        request.Data.GetInt();
        var packet = MessagePackSerializer.Deserialize<UserConnectPacket>(request.Data.GetRemainingBytes());
        if (UniteTheNorth.Version != packet.Version)
        {
            request.Reject(Encoding.UTF8.GetBytes($"Incompatible Version: Server is on {UniteTheNorth.Version}!"));
            return;
        }
        if (Password != null && packet.Password != Password)
        {
            request.Reject(Encoding.UTF8.GetBytes("Wrong Password!"));
            return;
        }
        // Add Valid Clients
        var id = 0;
        while (Clients.ContainsKey(id))
            id++;
        var newClient = Clients[id] = new Client(request.Accept(), packet.Username, id);
        // Send current Data
        foreach (var client in Clients.Values)
        {
            PacketManager.Send(client, new RegisterPlayerPacket(
                newClient.ID,
                newClient.Username
            ), DeliveryMethod.ReliableOrdered, Channels.System);
            PacketManager.Send(newClient, new RegisterPlayerPacket(
                client.ID,
                client.Username
            ), DeliveryMethod.ReliableOrdered, Channels.System);
        }
    }
}