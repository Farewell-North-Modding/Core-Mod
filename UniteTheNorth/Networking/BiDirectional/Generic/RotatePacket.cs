using LiteNetLib;
using MessagePack;
using UnityEngine;

namespace UniteTheNorth.Networking.BiDirectional.Generic;

[MessagePackObject]
public class RotatePacket : IBiDirectionalPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly Quaternion Rotation;
    
    public RotatePacket(int id, Quaternion rotation)
    {
        ID = id;
        Rotation = rotation;
    }

    public void HandlePacket()
    {
        NetworkRegistry.GetNetworkRotation(ID)?.ReceiveRotation(Rotation);
    }

    public void HandlePacket(Server.Client client)
    {
        PacketManager.SendToAll(this, DeliveryMethod.ReliableSequenced, Channels.Medium, client);
    }
}