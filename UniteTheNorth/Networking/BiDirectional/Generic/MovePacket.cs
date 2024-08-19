using LiteNetLib;
using MessagePack;
using UnityEngine;

namespace UniteTheNorth.Networking.BiDirectional.Generic;

[MessagePackObject]
public class MovePacket : IBiDirectionalPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly Vector3 Position;
    
    public MovePacket(int id, Vector3 position)
    {
        ID = id;
        Position = position;
    }
    
    public void HandlePacket()
    {
        NetworkRegistry.GetNetworkPosition(ID)?.ReceivePosition(Position);
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(this, DeliveryMethod.ReliableSequenced, Channels.Important, client);
    }
}