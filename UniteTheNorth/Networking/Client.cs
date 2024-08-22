using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using MelonLoader;
using MessagePack;
using UniteTheNorth.Networking.ServerBound;
using UniteTheNorth.Systems;
using UnityEngine;

namespace UniteTheNorth.Networking;

[RegisterTypeInIl2Cpp]
public class Client : MonoBehaviour, INetEventListener
{
    public static string Ip = "127.0.0.1";
    public static int Port = 4657;
    public static string Username = "Test";
    public static string Password = "";
    public static Client? Instance;

    public NetManager? NetClient;

    private void Start()
    {
        Instance = this;
        NetClient = new NetManager(this)
        {
            UpdateTime = 15,
            ChannelsCount = 4
        };
        var writer = new NetDataWriter();
        writer.Put(MessagePackSerializer.Serialize(new UserConnectPacket(UniteTheNorth.Version, Username, Password)));
        NetClient.Start();
        NetClient.Connect(new IPEndPoint(IPAddress.Parse(Ip), Port), writer);
        UniteTheNorth.Logger.Msg("[Client] Connecting to server...");
    }

    private void Update()
    {
        NetClient?.PollEvents();
    }

    public bool IsConnected()
    {
        return NetClient?.FirstPeer?.ConnectionState == ConnectionState.Connected;
    }

    private void OnDestroy()
    {
        NetClient?.Stop();
        PlayerManager.Clean();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        UniteTheNorth.Logger.Msg($"[Client] Connected to {peer.Address}");
        LocalNetworkManager.UpdateStatus("Loading save data...");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        UniteTheNorth.Logger.Msg($"[Client] Disconnected with reason: {disconnectInfo.Reason}");
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        UniteTheNorth.Logger.Warning($"[Client] Network Error: {socketError}");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        PacketManager.HandlePacket(reader);
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
        // Do absolutely nothing again :D
    }
}