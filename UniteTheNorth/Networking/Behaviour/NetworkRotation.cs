using LiteNetLib;
using MelonLoader;
using UniteTheNorth.Networking.BiDirectional.Generic;
using UnityEngine;

namespace UniteTheNorth.Networking.Behaviour;

/// <summary>
/// Registers this elements rotation in the network and syncs it
/// </summary>
[RegisterTypeInIl2Cpp]
public class NetworkRotation : NetworkBehaviour
{
    private const int LerpSpeed = 6;
    private Quaternion _rotationGoal;
    
    private Quaternion _lastRotation = Quaternion.identity;

    private void Start()
    {
        Initialize();
        _rotationGoal = transform.rotation;
        NetworkRegistry.RegisterRotation(this);
        Sender = SendData;
    }

    private void Update()
    {
        if(isHost)
            return;
        if (Quaternion.Angle(_rotationGoal, transform.rotation) > 3F)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _rotationGoal, LerpSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        DoUpdate();
    }

    private int SendData(int syncId)
    {
        if (!(Quaternion.Angle(_lastRotation, transform.rotation) > 3F)) return 2;
        _lastRotation = transform.rotation;
        PacketManager.Send(new RotatePacket(
            syncId,
            _lastRotation
        ), DeliveryMethod.ReliableSequenced);
        return 4;
    }

    public void ReceiveRotation(Quaternion rotation)
    {
        _rotationGoal = rotation;
    }
}