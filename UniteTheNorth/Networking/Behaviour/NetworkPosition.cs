using LiteNetLib;
using MelonLoader;
using UniteTheNorth.Networking.BiDirectional.Generic;
using UnityEngine;

namespace UniteTheNorth.Networking.Behaviour;

/// <summary>
/// Registers this elements position in the network and syncs it
/// </summary>
[RegisterTypeInIl2Cpp]
public class NetworkPosition : NetworkBehaviour
{
    private const int LerpSpeed = 5;
    private Vector3 _locationGoal;
    
    private static Vector3 _lastPosition = Vector3.zero;

    private new void Start()
    {
        base.Start();
        _locationGoal = transform.position;
        NetworkRegistry.RegisterPosition(this);
    }

    private void Update()
    {
        if (Vector3.Distance(_locationGoal, transform.position) > .1F)
        {
            transform.position = Vector3.Lerp(transform.position, _locationGoal, LerpSpeed * Time.deltaTime);
        }
    }

    protected override int SendData(int syncId)
    {
        if (!(Vector3.Distance(_lastPosition, transform.position) > .01F)) return 2;
        _lastPosition = transform.position;
        PacketManager.Send(new MovePacket(
            syncId,
            _lastPosition
        ), DeliveryMethod.ReliableSequenced, Channels.Important);
        return 4;
    }

    public void ReceivePosition(Vector3 position)
    {
        _locationGoal = position;
    }
}