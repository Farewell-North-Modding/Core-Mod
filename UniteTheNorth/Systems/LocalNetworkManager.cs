using UniteTheNorth.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniteTheNorth.Systems;

public static class LocalNetworkManager
{
    public static bool HostServer;
    public static string Address = "", Port = "4657", Username = "", Password = "";
    
    /// <summary>
    /// Starts client/server depending on the configuration
    /// </summary>
    public static void InitializeLocal()
    {
        if (Username == "" || Port == "" || (!HostServer && Address == ""))
        {
            UniteTheNorth.Logger.Error("Failed to initialize due to faulty address!");
            return;
        }
        if (!int.TryParse(Port, out var port))
        {
            UniteTheNorth.Logger.Error("Failed to initialize due to faulty port number!");
            return;
        }
        if (HostServer)
        {
            Server.Server.Port = port;
            Server.Server.Password = Password == "" ? null : Password;
            RunServer();
            Address = "127.0.0.1";
        }
        Client.Password = Password;
        Client.Port = port;
        Client.Ip = Address;
        Client.Username = Username;
        RunClient();
    }
    
    /// <summary>
    /// Starts a local server instance
    /// </summary>
    private static void RunServer()
    {
        var obj = new GameObject("UniteTheNorthServer");
        obj.AddComponent<Server.Server>();
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("Archipelago"));
    }

    /// <summary>
    /// Starts a client instance
    /// </summary>
    private static void RunClient()
    {
        var obj = new GameObject("UniteTheNorthClient");
        obj.AddComponent<Client>();
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("Archipelago"));
    }
}