using UniteTheNorth.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniteTheNorth.Systems;

public static class LocalNetworkManager
{
    /// <summary>
    /// Starts a local server instance
    /// </summary>
    public static void RunServer()
    {
        var obj = new GameObject("UniteTheNorthServer");
        obj.AddComponent<Server.Server>();
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("Archipelago"));
    }

    /// <summary>
    /// Starts a client instance
    /// </summary>
    public static void RunClient()
    {
        var obj = new GameObject("UniteTheNorthClient");
        obj.AddComponent<Client>();
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("Archipelago"));
    }
}