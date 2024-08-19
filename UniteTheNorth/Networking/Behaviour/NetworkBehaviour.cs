using Il2CppKBCore.Persistence;
using MelonLoader;
using UnityEngine;

namespace UniteTheNorth.Networking.Behaviour;

/// <summary>
/// A base class inheriting MonoBehaviour that handles sending packets and retrieving the sync id
/// </summary>
[RegisterTypeInIl2Cpp]
public abstract class NetworkBehaviour : MonoBehaviour
{
    private int _sendDelay = 10;
    private int? _syncId;

    /// <summary>
    /// Initializes the sync id, registers the object with the network registry and should always be called!
    /// </summary>
    protected void Start()
    {
        var uid = GetComponent<UniqueId>();
        if(uid == null)
            UniteTheNorth.Logger.Warning($"[Client] Missing UniqueId for NetworkBehaviour on {gameObject.name}");
        else
            _syncId = uid.ID.GetHashCode();
    }

    /// <summary>
    /// This function will update the sender and should be called if the SendData method isn't empty.
    /// </summary>
    protected void FixedUpdate()
    {
        if(_syncId == null)
            return;
        _sendDelay--;
        if(_sendDelay > 0)
            return;
        _sendDelay = SendData(_syncId ?? 0);
    }

    /// <summary>
    /// This function should send all data
    /// </summary>
    /// <returns>The amount to wait until sending again</returns>
    protected abstract int SendData(int syncId);
}