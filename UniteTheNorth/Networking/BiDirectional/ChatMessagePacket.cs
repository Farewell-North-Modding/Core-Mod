using LiteNetLib;
using MessagePack;

namespace UniteTheNorth.Networking.BiDirectional;

[MessagePackObject]
public class ChatMessagePacket : IBiDirectionalPacket
{
    [Key(0)] public readonly string Message;

    public ChatMessagePacket(string message)
    {
        Message = message;
    }
    
    public void HandlePacket()
    {
        UniteTheNorth.Logger.Msg($"[Client][Chat] {Message}");
    }

    public void HandlePacket(Server.Client client)
    {
        var newMessage = $"{client.Username}: {Message}";
        UniteTheNorth.Logger.Msg($"[Server][Chat] {newMessage}");
        PacketManager.SendToAll(new ChatMessagePacket(
            newMessage
        ), DeliveryMethod.ReliableOrdered, Channels.System);
    }
}