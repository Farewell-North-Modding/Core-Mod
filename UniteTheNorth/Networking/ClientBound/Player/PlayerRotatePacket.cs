using MessagePack;
using UniteTheNorth.Systems;
using UnityEngine;

namespace UniteTheNorth.Networking.ClientBound.Player;

[MessagePackObject]
public class PlayerRotatePacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly Quaternion Rotation;
    
    public PlayerRotatePacket(int id, Quaternion rotation)
    {
        ID = id;
        Rotation = rotation;
    }

    public void HandlePacket()
    {
        PlayerManager.RunOnPlayer(ID, player => player.ReceiveRotation(Rotation));
    }
}