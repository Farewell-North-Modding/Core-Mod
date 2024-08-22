using System.Text;
using Il2CppFarewellNorth.Coloring.Group;
using Il2CppFarewellNorth.Environment.Herds;
using Il2CppFarewellNorth.Environment.HiddenObjects;
using Il2CppFarewellNorth.Environment.Lighting;
using Il2CppFarewellNorth.Environment.MaskedObjects;
using Il2CppFarewellNorth.Levels.Collectables;
using Il2CppFarewellNorth.Levels.Lanterns;
using Il2CppFarewellNorth.Managers.Impl;
using Il2CppFarewellNorth.Misc;
using Il2CppFarewellNorth.Vehicles.Dock;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppKBCore.Abilities;
using Il2CppKBCore.NPC.Waypoints;
using Il2CppKBCore.Persistence;
using Il2CppKBCore.Persistence.Persistables;
using Il2CppKBCore.Platform.Achievements;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using UnityEngine;
using Exception = System.Exception;
using Object = UnityEngine.Object;
using String = Il2CppSystem.String;

namespace UniteTheNorth.Systems;

public static class MessagePackExtender
{
    public static void Initialize()
    {
        var resolver = CompositeResolver.Create(
            CustomResolver.Instance,
            NativeDecimalResolver.Instance,
            NativeGuidResolver.Instance,
            StandardResolver.Instance
        );
        var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
        MessagePackSerializer.DefaultOptions = options;
    }
}

public class CustomResolver : IFormatterResolver
{
    public static readonly IFormatterResolver Instance = new CustomResolver();
    
    public IMessagePackFormatter<T>? GetFormatter<T>()
    {
        if(typeof(T) == typeof(Vector3))
            return (IMessagePackFormatter<T>?) Vector3Formatter.Instance;
        if(typeof(T) == typeof(Quaternion))
            return (IMessagePackFormatter<T>?) QuaternionFormatter.Instance;
        if(typeof(T) == typeof(IDataStore))
            return (IMessagePackFormatter<T>?) DataStoreFormatter.Instance;
        return null;
    }
}

public class DataStoreFormatter : IMessagePackFormatter<IDataStore>
{
    private const bool Debug = false;
    public static readonly IMessagePackFormatter<IDataStore> Instance = new DataStoreFormatter();
    
    public void Serialize(ref MessagePackWriter writer, IDataStore value, MessagePackSerializerOptions options)
    {
        var trueVal = value.Cast<DataStore>();
        writer.WriteArrayHeader(trueVal.KeyCount);
        foreach (var entry in trueVal._gameData)
        {
            writer.WriteString(Encoding.UTF8.GetBytes(entry.Key));
            try
            {
                var val = entry.Value.Unbox<bool>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending bool {val}");
                writer.Write((byte) (val ? 1 : 2));
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                if (entry.Value.GetIl2CppType().FullName == "System.String")
                {
                    var val = new String(entry.Value.Pointer);
                    if(Debug)
                        UniteTheNorth.Logger.Msg($"[Debug] Sending string {((string) val)}");
                    writer.Write((byte) 3);
                    writer.WriteString(Encoding.UTF8.GetBytes((string) val));
                    continue;
                }
            }
            catch (Exception e) { UniteTheNorth.Logger.Msg(e); /* ignored */ }
            try
            {
                var val = entry.Value.Cast<CollectableGroup.PersistedStates>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending PersistedStates {val}");
                writer.Write((byte) 10);
                writer.WriteInt32(val.AllStates[0].Collected);
                writer.WriteInt32(val.AllStates[0].Available);
                writer.WriteInt32(val.AllStates[0].Total);
                writer.WriteInt32(val.AllStates[1].Collected);
                writer.WriteInt32(val.AllStates[1].Available);
                writer.WriteInt32(val.AllStates[1].Total);
                writer.WriteInt32(val.AllStates[2].Collected);
                writer.WriteInt32(val.AllStates[2].Available);
                writer.WriteInt32(val.AllStates[2].Total);
                writer.WriteInt32(val.AllStates[3].Collected);
                writer.WriteInt32(val.AllStates[3].Available);
                writer.WriteInt32(val.AllStates[3].Total);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<MaskedObjectState>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending MaskedObjectState {val}");
                writer.Write((byte) 11);
                writer.Write((byte) val);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<LanternState>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending LanternState {val}");
                writer.Write((byte) 12);
                writer.Write(val.Activated);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<HiddenObject.State>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending HiddenObject.State {val}");
                writer.Write((byte) 13);
                writer.Write((byte) val);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<NPCWaypointSetState>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending NPCWaypointSetState {val}");
                writer.Write((byte) 14);
                writer.Write(val.HasActivated);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<GlobalLightSourcePersistable.Data>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending GlobalLightSourcePersistable.State {val}");
                writer.Write((byte) 15);
                writer.Write(val.HasActivated);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<ColorNodeGroupState>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending ColorNodeGroupState {val}");
                writer.Write((byte) 16);
                writer.Write(val.HasBeenActivated);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<HerdMemberState>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending HerdMemberState {val}");
                writer.Write((byte) 17);
                writer.Write((byte) val);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<DockCheckpointLookAtDirectionManager.Direction>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending HerdMemberState {val}");
                writer.Write((byte) 18);
                writer.Write((byte) val);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Unbox<TransformPersistableData>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending TransformPersistableData {val}");
                writer.Write((byte) 19);
                writer.Write(val.Position.x);
                writer.Write(val.Position.y);
                writer.Write(val.Position.z);
                writer.Write(val.EulerAngles.x);
                writer.Write(val.EulerAngles.y);
                writer.Write(val.EulerAngles.z);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Cast<WorldZoneEnabledHistoryPersistable.State>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending WorldZoneEnabledHistoryPersistable.State {val}");
                writer.Write((byte) 20);
                writer.WriteArrayHeader(val.EnabledZoneIDs.Count);
                foreach (var id in val.EnabledZoneIDs)
                    writer.WriteInt32(id);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Cast<AbilitySet.State>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending AbilitySet.State {val}");
                writer.Write((byte) 21);
                writer.WriteArrayHeader(val.AvailableAbilities.Count);
                foreach (var id in val.AvailableAbilities)
                    writer.WriteInt32(id);
                continue;
            }
            catch (Exception) { /* ignored */ }
            try
            {
                var val = entry.Value.Cast<AchievementsState.State>();
                if(Debug)
                    UniteTheNorth.Logger.Msg($"[Debug] Sending AchievementsState.State {val}");
                writer.Write((byte) 22);
                writer.WriteArrayHeader(val.UnlockedAchievements.Count);
                foreach (var id in val.UnlockedAchievements)
                    writer.WriteString(Encoding.UTF8.GetBytes(id.AchievementId));
                continue;
            }
            catch (Exception) { /* ignored */ }
            writer.Write((byte)0);
            UniteTheNorth.Logger.Msg($"[Save Formatter] Unknown data type in store formatter: {entry.Value.GetIl2CppType().FullName}");
        }
    }

    public IDataStore Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var manager = Object.FindObjectOfType<PersistenceManager>();
        var store = new DataStore(manager.SettingsDataStore.Cast<DataStore>()._driver, false);
        var entries = reader.ReadArrayHeader();
        store._gameData.Clear();
        for (var i = 0; i < entries; i++)
        {
            var key = reader.ReadString();
            var id = reader.ReadByte();
            if(Debug)
                UniteTheNorth.Logger.Msg($"[Debug] Receiving key {key} type {id}");
            switch (id)
            {
                case 1:
                    store._gameData.Add(key, true);
                    break;
                case 2:
                    store._gameData.Add(key, false);
                    break;
                case 3:
                    store._gameData.Add(key, reader.ReadString());
                    break;
                case 10:
                    var states = new CollectableGroup.PersistedStates
                    {
                        AllStates = new Il2CppStructArray<CollectableGroup.State>(4)
                        {
                            [0] = new(CollectableType.Wisp, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                            [1] = new(CollectableType.Lighthouse, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                            [2] = new(CollectableType.RestSpot, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                            [3] = new(CollectableType.MusicNote, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32())
                        }
                    };
                    store._gameData.Add(key, states);
                    break;
                case 11:
                    var mosState = (MaskedObjectState) reader.ReadByte();
                    if (Il2CppSystem.Enum.TryParse(Il2CppType.Of<MaskedObjectState>(), mosState.ToString(), out var mosStateBoxed))
                    {
                        store._gameData.Add(key, mosStateBoxed);
                    } else
                        UniteTheNorth.Logger.Msg($"[Save Formatter] Couldn't parse MaskedObjectState {mosState} for {key}");
                    break;
                case 12:
                    var lsState = new LanternState
                    {
                        Activated = reader.ReadBoolean()
                    };
                    store._gameData.Add(key, lsState.BoxIl2CppObject());
                    break;
                case 13:
                    var hoState = (HiddenObject.State) reader.ReadByte();
                    if (Il2CppSystem.Enum.TryParse(Il2CppType.Of<HiddenObject.State>(), hoState.ToString(), out var hoStateBoxed))
                    {
                        store._gameData.Add(key, hoStateBoxed);
                    } else
                        UniteTheNorth.Logger.Msg($"[Save Formatter] Couldn't parse HiddenObject.State {hoState} for {key}");
                    break;
                case 14:
                    var npcState = new NPCWaypointSetState
                    {
                        HasActivated = reader.ReadBoolean()
                    };
                    store._gameData.Add(key, npcState.BoxIl2CppObject());
                    break;
                case 15:
                    var lightData = new GlobalLightSourcePersistable.Data
                    {
                        HasActivated = reader.ReadBoolean()
                    };
                    store._gameData.Add(key, lightData.BoxIl2CppObject());
                    break;
                case 16:
                    var nodeState = new ColorNodeGroupState
                    {
                        HasBeenActivated = reader.ReadBoolean()
                    };
                    store._gameData.Add(key, nodeState.BoxIl2CppObject());
                    break;
                case 17:
                    var headState = (HerdMemberState) reader.ReadByte();
                    if (Il2CppSystem.Enum.TryParse(Il2CppType.Of<HerdMemberState>(), headState.ToString(), out var headStateBoxed))
                    {
                        store._gameData.Add(key, headStateBoxed);
                    } else
                        UniteTheNorth.Logger.Msg($"[Save Formatter] Couldn't parse HerdMemberState {headState} for {key}");
                    break;
                case 18:
                    var directionState = (DockCheckpointLookAtDirectionManager.Direction) reader.ReadByte();
                    if (Il2CppSystem.Enum.TryParse(Il2CppType.Of<DockCheckpointLookAtDirectionManager.Direction>(), directionState.ToString(), out var directionStateBoxed))
                    {
                        store._gameData.Add(key, directionStateBoxed);
                    } else
                        UniteTheNorth.Logger.Msg($"[Save Formatter] Couldn't parse HerdMemberState {directionState} for {key}");
                    break;
                case 19:
                    var data = new TransformPersistableData
                    {
                        Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
                        EulerAngles = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
                    };
                    store._gameData.Add(key, data.BoxIl2CppObject());
                    break;
                case 20:
                    var worldState = new WorldZoneEnabledHistoryPersistable.State();
                    var wStateCount = reader.ReadArrayHeader();
                    worldState.EnabledZoneIDs = new Il2CppSystem.Collections.Generic.List<int>(wStateCount);
                    for(var j = 0; j < wStateCount; j++) 
                        worldState.EnabledZoneIDs.Add(reader.ReadInt32());
                    store._gameData.Add(key, worldState);
                    break;
                case 21:
                    var abilityState = new AbilitySet.State();
                    var wAbilityCount = reader.ReadArrayHeader();
                    abilityState.AvailableAbilities = new Il2CppSystem.Collections.Generic.List<int>(wAbilityCount);
                    for(var j = 0; j < wAbilityCount; j++) 
                        abilityState.AvailableAbilities.Add(reader.ReadInt32());
                    store._gameData.Add(key, abilityState);
                    break;
                case 22:
                    var wAchievementCount = reader.ReadArrayHeader();
                    for(var k = 0; k < wAchievementCount; k++)
                        UniteTheNorth.Logger.Msg($"[Save Formatter] Ignoring Achievement {reader.ReadString()}");
                    break;
                case 0:
                    break;
            }
        }
        return store.Cast<IDataStore>();
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