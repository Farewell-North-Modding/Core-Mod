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
            Client.Instance!.NetClient?.SendToAll(data, (byte) channel, method);
    }
    
    public static void HandlePacket(NetPacketReader reader)
    {
        var packetId = reader.GetInt();
        var packetData = reader.GetRemainingBytes();
        foreach (var type in from pair in ClientBoundMap where pair.Value == packetId select pair.Key)
        {
            if (MessagePackSerializer.Deserialize(type, packetData) is IClientBoundPacket packet)
            {
                packet.HandlePacket();
            }
        }
        // Log
    }

    public static void HandlePacket(Client client, NetPacketReader reader)
    {
        var packetId = reader.GetInt();
        var packetData = reader.GetRemainingBytes();
        foreach (var type in from pair in ServerBoundMap where pair.Value == packetId select pair.Key)
        {
            if (MessagePackSerializer.Deserialize(type, packetData) is IServerBoundPacket packet)
            {
                packet.HandlePacket(client);
            }
        }
        // Log
    }
}