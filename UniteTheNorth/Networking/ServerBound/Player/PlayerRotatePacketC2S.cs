using LiteNetLib;
using MessagePack;
using UniteTheNorth.Networking.ClientBound.Player;
using UnityEngine;

namespace UniteTheNorth.Networking.ServerBound.Player;

[MessagePackObject]
public class PlayerRotatePacketC2S : IServerBoundPacket
{
    [Key(0)] public readonly Quaternion Rotation;
    
    public PlayerRotatePacketC2S(Quaternion rotation)
    {
        Rotation = rotation;
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(new PlayerRotatePacket(
            client.ID,
            Rotation
        ), DeliveryMethod.ReliableSequenced, Channels.Medium, client);
    }
}