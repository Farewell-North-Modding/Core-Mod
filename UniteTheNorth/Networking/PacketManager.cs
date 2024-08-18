using LiteNetLib;
using LiteNetLib.Utils;
using MessagePack;
using UniteTheNorth.Networking.BiDirectional;
using UniteTheNorth.Networking.ClientBound;
using UniteTheNorth.Networking.ClientBound.Player;
using UniteTheNorth.Networking.ServerBound;
using UniteTheNorth.Networking.ServerBound.Player;

namespace UniteTheNorth.Networking;

public static class PacketManager
{
    /**
     * Register all your client bound packets in this map for them to be recognized in the protocol. Please use IDs that only your mod could ever have or use our ID generator.
     */
    public static readonly Dictionary<Type, int> ClientBoundMap = new()
    {
        // Bi Directional
        { typeof(KeepAlivePacket), 0 },
        // Client Bound 
        { typeof(RegisterPlayerPacket), 1 },
        { typeof(UnregisterPlayerPacket), 2 },
        { typeof(PlayerMovePacket), 10 },
        { typeof(PlayerRotatePacket), 11 },
        { typeof(PlayerAnimatePacket), 12 },
    };
    
    /**
     * Register all your server bound packets in this map for them to be recognized in the protocol. Please use IDs that only your mod could ever have or use our ID generator.
     */
    public static readonly Dictionary<Type, int> ServerBoundMap = new()
    {
        // Bi Directional
        { typeof(KeepAlivePacket), 0 },
        // Server Bound
        { typeof(UserConnectPacket), -1 },
        { typeof(PlayerMovePacketC2S), 10 },
        { typeof(PlayerRotatePacketC2S), 11 },
        { typeof(PlayerAnimatePacketC2S), 12 },
    };
    
    public static NetDataWriter SerializePacket(object packet)
    {
        if (!ClientBoundMap.TryGetValue(packet.GetType(), out var id) && !ServerBoundMap.TryGetValue(packet.GetType(), out id))
            throw new NotImplementedException($"Invalid Packet Type: {packet.GetType().Name}");
        var writer = new NetDataWriter();
        writer.Put(id);
        writer.Put(MessagePackSerializer.Serialize(packet));
        return writer;
    }

    public static void Send(IServerBoundPacket packet, DeliveryMethod method = DeliveryMethod.ReliableUnordered, Channels channel = Channels.Medium)
    {
        var data = SerializePacket(packet);
        if (Client.Instance?.IsConnected() == true)
            Client.Instance.NetClient?.SendToAll(data, (byte) channel, method);
    }

    public static void Send(Server.Client client, IClientBoundPacket packet, DeliveryMethod method = DeliveryMethod.ReliableUnordered, Channels channel = Channels.Medium)
    {
        var data = SerializePacket(packet);
        client.Peer.Send(data, (byte) channel, method);
    }

    public static void SendToAll(IClientBoundPacket packet, DeliveryMethod method = DeliveryMethod.ReliableUnordered, Channels channel = Channels.Medium, Server.Client? exclude = null)
    {
        var data = SerializePacket(packet);
        if(exclude != null)
            Server.Server.Instance?.NetServer?.SendToAll(data, (byte) channel, method, exclude.Peer);
        else
            Server.Server.Instance?.NetServer?.SendToAll(data, (byte) channel, method);
    }

    public static void HandlePacket(NetPacketReader reader)
    {
        try
        {
            var packetId = reader.GetInt();
            var packetData = reader.GetRemainingBytes();
            foreach (var type in from pair in ClientBoundMap where pair.Value == packetId select pair.Key)
            {
                if (MessagePackSerializer.Deserialize(type, packetData) is not IClientBoundPacket packet) continue;
                packet.HandlePacket();
                return;
            }
            UniteTheNorth.Logger.Warning("[Client] Couldn't handle packet with ID: " + packetId);
        }
        catch (Exception e)
        {
            UniteTheNorth.Logger.Warning(e);
            throw;
        }
    }

    public static void HandlePacket(Server.Client client, NetPacketReader reader)
    {
        try
        {
            var packetId = reader.GetInt();
            var packetData = reader.GetRemainingBytes();
            foreach (var type in from pair in ServerBoundMap where pair.Value == packetId select pair.Key)
            {
                if (MessagePackSerializer.Deserialize(type, packetData) is not IServerBoundPacket packet) continue;
                packet.HandlePacket(client);
                return;
            }
            UniteTheNorth.Logger.Warning("[Server] Couldn't handle packet with ID: " + packetId);
        }
        catch (Exception e)
        {
            UniteTheNorth.Logger.Warning(e);
            throw;
        }
    }
}