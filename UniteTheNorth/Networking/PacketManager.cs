using LiteNetLib;
using LiteNetLib.Utils;
using MessagePack;
using UniteTheNorth.Networking.BiDirectional;
using UniteTheNorth.Networking.ClientBound.Player;
using UniteTheNorth.Networking.ServerBound.Player;

namespace UniteTheNorth.Networking;

public static class PacketManager
{
    /// <summary>
    /// Register all your client bound packets in this map for them to be recognized in the protocol. Please use IDs that only your mod could ever have or use our ID generator.
    /// </summary>
    public static readonly Dictionary<Type, int> ClientBoundMap = new()
    {
        // Bi Directional (0-9)
        { typeof(KeepAlivePacket), 0 },
        { typeof(ChatMessagePacket), 1 },
        // Client Bound (System 10-19; Play 20+)
        { typeof(RegisterPlayerPacket), 10 },
        { typeof(UnregisterPlayerPacket), 11 },
        { typeof(PlayerMovePacket), 20 },
        { typeof(PlayerRotatePacket), 21 },
        { typeof(PlayerAnimateBoolPacket), 22 },
        { typeof(PlayerAnimateFloatPacket), 23 },
        { typeof(PlayerAnimateIntPacket), 24 },
    };
    
    /// <summary>
    /// Register all your server bound packets in this map for them to be recognized in the protocol. Please use IDs that only your mod could ever have or use our ID generator.
    /// </summary>
    public static readonly Dictionary<Type, int> ServerBoundMap = new()
    {
        // Bi Directional (0-9)
        { typeof(KeepAlivePacket), 0 },
        { typeof(ChatMessagePacket), 1 },
        // Server Bound (System 10-19; Play 20+)
        { typeof(PlayerMovePacketC2S), 20 },
        { typeof(PlayerRotatePacketC2S), 21 },
        { typeof(PlayerAnimateBoolPacketC2S), 22 },
        { typeof(PlayerAnimateFloatPacketC2S), 23 },
        { typeof(PlayerAnimateIntPacketC2S), 24 },
    };
    
    /// <summary>
    /// Serializes a packet and joins it with its corresponding protocol ID into a LiteNetLib NetDataWriter
    /// </summary>
    /// <param name="packet">The packet to serialize and numerate</param>
    /// <returns>NetDataWriter containing both packet ID and serialized packet</returns>
    /// <exception cref="NotImplementedException">Thrown if the packets type isn't registered in the protocol</exception>
    public static NetDataWriter SerializePacket(object packet)
    {
        if (!ClientBoundMap.TryGetValue(packet.GetType(), out var id) && !ServerBoundMap.TryGetValue(packet.GetType(), out id))
            throw new NotImplementedException($"Invalid Packet Type: {packet.GetType().Name}");
        var writer = new NetDataWriter();
        writer.Put(id);
        writer.Put(MessagePackSerializer.Serialize(packet.GetType(), packet));
        return writer;
    }

    /// <summary>
    /// Sends a packet to the server
    /// </summary>
    /// <param name="packet">The packet to send</param>
    /// <param name="method">The LiteNetLib delivery method</param>
    /// <param name="channel">The channel to send the packet on</param>
    public static void Send(IServerBoundPacket packet, DeliveryMethod method = DeliveryMethod.ReliableUnordered, Channels channel = Channels.Medium)
    {
        var data = SerializePacket(packet);
        if (Client.Instance?.IsConnected() == true)
            Client.Instance.NetClient?.SendToAll(data, (byte) channel, method);
    }

    /// <summary>
    /// Sends a packet to a client
    /// </summary>
    /// <param name="client">The client to send the packet to</param>
    /// <param name="packet">The packet to send</param>
    /// <param name="method">The LiteNetLib delivery method</param>
    /// <param name="channel">The channel to send the packet on</param>
    public static void Send(Server.Client client, IClientBoundPacket packet, DeliveryMethod method = DeliveryMethod.ReliableUnordered, Channels channel = Channels.Medium)
    {
        var data = SerializePacket(packet);
        client.Peer.Send(data, (byte) channel, method);
    }

    /// <summary>
    /// Broadcasts a packet to all clients
    /// </summary>
    /// <param name="packet">The packet to send</param>
    /// <param name="method">The LiteNetLib delivery method</param>
    /// <param name="channel">The channel to send the packet on</param>
    /// <param name="exclude">The client that should be excluded from the broadcast</param>
    public static void SendToAll(IClientBoundPacket packet, DeliveryMethod method = DeliveryMethod.ReliableUnordered, Channels channel = Channels.Medium, Server.Client? exclude = null)
    {
        var data = SerializePacket(packet);
        if(exclude != null)
            Server.Server.Instance?.NetServer?.SendToAll(data, (byte) channel, method, exclude.Peer);
        else
            Server.Server.Instance?.NetServer?.SendToAll(data, (byte) channel, method);
    }

    /// <summary>
    /// Handles a client-bound packet
    /// </summary>
    /// <param name="reader">The LiteNetLib reader</param>
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

    /// <summary>
    /// Handles a server-bound packet
    /// </summary>
    /// <param name="client">The client the packet was sent from</param>
    /// <param name="reader">The LiteNetLib reader</param>
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