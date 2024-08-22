using Il2CppFarewellNorth.Managers.Impl;
using Il2CppFarewellNorth.UI.Menu.Main;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppKBCore.Persistence;
using MessagePack;
using UniteTheNorth.Systems;
using UniteTheNorth.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniteTheNorth.Networking.ClientBound;

[MessagePackObject]
public class PlayDataPacket : IClientBoundPacket
{
    [Key(0)] public readonly int ID;
    [Key(1)] public readonly IDataStore Collectibles;
    [Key(2)] public readonly IDataStore GameSave;
    [Key(3)] public readonly IDataStore Achievements;

    public PlayDataPacket(int id, IDataStore collectibles, IDataStore gameSave, IDataStore achievements)
    {
        ID = id;
        Collectibles = collectibles;
        GameSave = gameSave;
        Achievements = achievements;
    }

    public void HandlePacket()
    {
        if (LocalNetworkManager.HostServer)
        {
            UniteTheNorth.Logger.Msg($"[Client] Received local id ({ID})");
            UniteTheNorth.OnGameSceneLoaded.Add(() => NetworkWrapper.CreatePlayerWrapper(ID));
            return;
        }
        UniteTheNorth.Logger.Msg($"[Client] Received local id ({ID}) and save information. Launching...");
        LocalNetworkManager.UpdateStatus("Loading...");
        var manager = Object.FindObjectOfType<PersistenceManager>();
        manager._collectables = new Il2CppReferenceArray<IDataStore>(5)
        {
            [0] = manager._collectables[0],
            [1] = manager._collectables[1],
            [2] = manager._collectables[2],
            [3] = manager._collectables[3],
            [4] = Collectibles
        };
        manager._gameSaves = new Il2CppReferenceArray<IDataStore>(5)
        {
            [0] = manager._gameSaves[0],
            [1] = manager._gameSaves[1],
            [2] = manager._gameSaves[2],
            [3] = manager._gameSaves[3],
            [4] = GameSave
        };
        manager._achievements = new Il2CppReferenceArray<IDataStore>(5)
        {
            [0] = manager._achievements[0],
            [1] = manager._achievements[1],
            [2] = manager._achievements[2],
            [3] = manager._achievements[3],
            [4] = GameSave
        };
        manager.ContinueSave(4);
        GameObject.Find("Main Menu").GetComponent<MainMenuController>().GoToGameplayScene();
        UniteTheNorth.OnGameSceneLoaded.Add(() => NetworkWrapper.CreatePlayerWrapper(ID));
    }
}