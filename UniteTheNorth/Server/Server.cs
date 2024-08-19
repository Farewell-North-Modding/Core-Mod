using System.Net;
using System.Net.Sockets;
using System.Text;
using LiteNetLib;
using MelonLoader;
using MessagePack;
using UniteTheNorth.Networking;
using UniteTheNorth.Networking.BiDirectional;
using UniteTheNorth.Networking.ClientBound;
using UniteTheNorth.Networking.ClientBound.Player;
using UniteTheNorth.Networking.ServerBound;
using UnityEngine;

namespace UniteTheNorth.Server;

[RegisterTypeInIl2Cpp]
public class Server : MonoBehaviour, INetEventListener
{
    private const int Port = 4657;
    private const string? Password = null;
    public static Server? Instance;
    
    public NetManager? NetServer;
    private readonly Dictionary<int, Client> _clients = new();
    private int _keepAliveDelay = 300;
    
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

    private void FixedUpdate()
    {
        _keepAliveDelay--;
        if(_keepAliveDelay > 0)
            return;
        _keepAliveDelay = 300;
        PacketManager.SendToAll(new KeepAlivePacket(), DeliveryMethod.Unreliable, Channels.Unimportant);
    }

    private void OnDestroy()
    {
        NetServer?.Stop();
    }

    private bool TryGetClient(NetPeer peer, out Client? client)
    {
        client = _clients.Values.FirstOrDefault(client => Equals(client.Peer, peer));
        return client != null;
    }

    public void OnPeerConnected(NetPeer peer)
    {
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        if (TryGetClient(peer, out var client))
        {
            _clients.Remove(client!.ID);
            PacketManager.SendToAll(new ChatMessagePacket($"{client.Username} left the game"));
            UniteTheNorth.Logger.Msg($"[Server] User {client.Username} disconnected: {disconnectInfo.Reason}");
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
        while (_clients.ContainsKey(id))
            id++;
        var newClient = _clients[id] = new Client(request.Accept(), packet.Username, id);
        // Send current Data
        foreach (var client in _clients.Values.Where(client => client != newClient))
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
        PacketManager.SendToAll(new ChatMessagePacket($"{newClient.Username} joined the game"));
    }
}