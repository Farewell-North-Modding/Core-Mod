using Il2CppKBCore.Persistence;
using MelonLoader;
using UnityEngine;

namespace UniteTheNorth.Networking.Behaviour;

/// <summary>
/// A base class inheriting MonoBehaviour that handles sending packets and retrieving the sync id
/// </summary>
[RegisterTypeInIl2Cpp]
public class NetworkBehaviour : MonoBehaviour
{
    public bool isHost;
    private int _sendDelay = 10;
    private int? _syncId;

    /// <summary>
    /// This function should send all data
    /// </summary>
    /// <returns>The amount to wait until sending again</returns>
    protected Func<int, int>? Sender = null;

    /// <summary>
    /// Initializes the sync id, registers the object with the network registry and should always be called!
    /// </summary>
    protected void Initialize()
    {
        if(_syncId != null)
            return;
        if (TryGetComponent(out UniqueId uid))
            _syncId = uid.ID.GetHashCode();
        else
            UniteTheNorth.Logger.Warning($"[Client] Missing UniqueId or Overwrite for NetworkBehaviour on {gameObject.name}");
        UniteTheNorth.Logger.Msg($"[Client] NetworkId: {_syncId}");
    }

    /// <summary>
    /// This function will update the sender and should be called if the SendData method isn't empty.
    /// </summary>
    protected void DoUpdate()
    {
        if(!isHost || _syncId == null || Sender == null)
            return;
        _sendDelay--;
        if(_sendDelay > 0)
            return;
        _sendDelay = Sender(_syncId ?? 0);
    }

    /// <summary>
    /// Overwrites this elements sync id
    /// </summary>
    /// <param name="syncId">The new sync id</param>
    public void OverwriteSyncId(int syncId)
    {
        _syncId = syncId;
    }

    /// <summary>
    /// Gets the elements sync id
    /// </summary>
    /// <returns>The sync id based on the UniqueId component</returns>
    public int GetSyncId()
    {
        return _syncId ?? -1;
    }
}