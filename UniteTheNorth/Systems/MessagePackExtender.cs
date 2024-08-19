using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using UnityEngine;

namespace UniteTheNorth.Systems;

public static class MessagePackExtender
{
    public static void Initialize()
    {
        var resolver = CompositeResolver.Create(
            UnityResolver.Instance,
            NativeDecimalResolver.Instance,
            NativeGuidResolver.Instance,
            StandardResolver.Instance
        );
        var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
        MessagePackSerializer.DefaultOptions = options;
    }
}

public class UnityResolver : IFormatterResolver
{
    public static readonly IFormatterResolver Instance = new UnityResolver();
    
    public IMessagePackFormatter<T>? GetFormatter<T>()
    {
        if(typeof(T) == typeof(Vector3))
            return (IMessagePackFormatter<T>?) Vector3Formatter.Instance;
        if(typeof(T) == typeof(Quaternion))
            return (IMessagePackFormatter<T>?) QuaternionFormatter.Instance;
        return null;
    }
}

public class Vector3Formatter : IMessagePackFormatter<Vector3>
{
    public static readonly IMessagePackFormatter<Vector3> Instance = new Vector3Formatter();
    
    public void Serialize(ref MessagePackWriter writer, Vector3 value, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(3);
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public Vector3 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.ReadArrayHeader() != 3)
        {
            throw new MessagePackSerializationException("Invalid Unity:Vector3 data.");
        }
        return new Vector3(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle()
        );
    }
}

public class QuaternionFormatter : IMessagePackFormatter<Quaternion>
{
    public static readonly IMessagePackFormatter<Quaternion> Instance = new QuaternionFormatter();
    
    public void Serialize(ref MessagePackWriter writer, Quaternion value, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(4);
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }

    public Quaternion Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.ReadArrayHeader() != 4)
        {
            throw new MessagePackSerializationException("Invalid Unity:Quaternion data.");
        }
        return new Quaternion(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle()
        );
    }
}