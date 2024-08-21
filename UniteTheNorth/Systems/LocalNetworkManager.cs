using Il2CppRTLTMPro;
using UniteTheNorth.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UniteTheNorth.Systems;

public static class LocalNetworkManager
{
    public static bool HostServer;
    public static string Address = "", Port = "4657", Username = "", Password = "";

    private static RTLTextMeshPro? _statusComp;

    /// <summary>
    /// Internal method that gets called on scene load
    /// </summary>
    /// <param name="sceneName">The scene name of the loaded scene</param>
    public static void OnSceneLoad(string sceneName)
    {
        if(sceneName != "Archipelago" || !HostServer)
            return;
        InitializeLocal();
    }
    
    /// <summary>
    /// Starts client/server depending on the configuration
    /// </summary>
    public static void InitializeLocal()
    {
        PlayerManager.Clean();
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
        Object.DontDestroyOnLoad(obj);
        obj.AddComponent<Server.Server>();
    }

    /// <summary>
    /// Starts a client instance
    /// </summary>
    private static void RunClient()
    {
        var obj = new GameObject("UniteTheNorthClient");
        Object.DontDestroyOnLoad(obj);
        obj.AddComponent<Client>();
    }

    /// <summary>
    /// Supplies the manager with an element that can be used later on to update the connection status
    /// </summary>
    /// <param name="status">The text element to use</param>
    public static void SupplyStatusLabel(RTLTextMeshPro status)
    {
        _statusComp = status;
    }

    /// <summary>
    /// Updates the status element if it is provided
    /// </summary>
    /// <param name="status">The status text to use</param>
    public static void UpdateStatus(string status)
    {
        if(_statusComp == null || !_statusComp)
            return;
        _statusComp!.originalText = status;
        _statusComp.text = status;
        _statusComp.SetText(status);
    }
}