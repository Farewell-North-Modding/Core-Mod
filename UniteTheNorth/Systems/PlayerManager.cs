﻿using FarewellCore.Tools;
using UniteTheNorth.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniteTheNorth.Systems;

public static class PlayerManager
{
    private static bool _isLoading = true;
    private static readonly Dictionary<int, PrePlayPlayerCache> PrePlayCache = new();
    private static readonly Dictionary<int, NetPlayer> PlayerCache = new();
    
    /// <summary>
    /// A private method that creates a new NetPlayer from a player dummy (core type)
    /// </summary>
    /// <returns>The newly created NetPlayer instance</returns>
    private static NetPlayer CreateNetPlayer()
    {
        var newObject = GameplayFinder.FindPlayer()?.CreatePlayerDummy().GetGameObject();
        newObject!.SetActive(true);
        return newObject.AddComponent<NetPlayer>();
    }
    
    /// <summary>
    /// This method is called once the game loads and goes over all the pre-play cache entries
    /// </summary>
    public static void MainSceneLoaded()
    {
        _isLoading = false;
        foreach (var cached in PrePlayCache.Values)
        {
            RegisterPlayer(cached.ID, cached.Username);
            foreach (var action in cached.ActionQueue)
            {
                action.Invoke(PlayerCache[cached.ID]);
            }
        }
    }

    /// <summary>
    /// Runs an action on the player once the game is loaded
    /// </summary>
    /// <param name="id">The players ID</param>
    /// <param name="action">The action to run</param>
    public static void RunOnPlayer(int id, Action<NetPlayer> action)
    {
        if(_isLoading)
            if(PrePlayCache.TryGetValue(id, out var value))
                value.ActionQueue.Add(action);
            else
                UniteTheNorth.Logger.Msg($"[Client] Tried adding action to unknown player {id}");
        else
            if(PlayerCache.TryGetValue(id, out var value))
                action.Invoke(value);
            else
                UniteTheNorth.Logger.Msg($"[Client] Tried running action on unknown player {id}");
            action.Invoke(PlayerCache[id]);
    }

    /// <summary>
    /// Registers a player to the pre-game or player registry
    /// </summary>
    /// <param name="id">The players ID</param>
    /// <param name="username">The players Username</param>
    public static void RegisterPlayer(int id, string username)
    {
        if (_isLoading)
        {
            UniteTheNorth.Logger.Msg($"[Client] Precaching player {id} with username {username}");
            PrePlayCache.Add(id, new PrePlayPlayerCache(id, username));
            return;
        }
        var player = CreateNetPlayer();
        player.ReceivePlayerInfo(username);
        PlayerCache[id] = player;
        UniteTheNorth.Logger.Msg($"[Client] Registered player {id} with username {username}");
    }

    /// <summary>
    /// Removes a player from the pre-game or player registry
    /// </summary>
    /// <param name="id">The players ID</param>
    public static void UnregisterPlayer(int id)
    {
        if(PlayerCache.TryGetValue(id, out var player))
            Object.Destroy(player.gameObject);
        PrePlayCache.Remove(id);
        PlayerCache.Remove(id);
        UniteTheNorth.Logger.Msg($"[Client] Unregistered player {id}");
    }

    /// <summary>
    /// Cleans the manager for use in a new connection
    /// </summary>
    public static void Clean()
    {
        _isLoading = true;
        PrePlayCache.Clear();
        PlayerCache.Clear();
    }

    /// <summary>
    /// This class is used for caching players and corresponding actions so they can be applied when the game has loaded.
    /// </summary>
    private class PrePlayPlayerCache
    {
        public readonly int ID;
        public readonly string Username;
        public readonly List<Action<NetPlayer>> ActionQueue = new();

        public PrePlayPlayerCache(int id, string username)
        {
            ID = id;
            Username = username;
        }
    }

    /// <summary>
    /// Updates the active state of all NetPlayers
    /// </summary>
    public static void UpdateState()
    {
        foreach (var player in PlayerCache.Values)
        {
            player.gameObject.SetActive(true);
        }
    }
}