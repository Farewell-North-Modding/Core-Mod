using LiteNetLib;
using MelonLoader;
using UniteTheNorth.Networking.BiDirectional.Generic;
using UniteTheNorth.Tools;
using UnityEngine;

namespace UniteTheNorth.Networking.Behaviour;

/// <summary>
/// Registers this elements animator in the network and syncs it
/// </summary>
[RegisterTypeInIl2Cpp]
public class NetworkAnimator : NetworkBehaviour
{
    private Animator? _animator;
    private AnimatorFloatLerp? _floatLerp;
    
    private static bool _animatorInitialized;
    private static bool[]? _lastBools;
    private static float[]? _lastFloats;
    private static int[]? _lastInts;

    private new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _floatLerp = new AnimatorFloatLerp(_animator);
        NetworkRegistry.RegisterAnimator(this);
        Sender = SendData;
    }

    private void Update()
    {
        _floatLerp?.Update();
    }

    private int SendData(int syncId)
    {
        var parameters = _animator?.parameters;
        if(parameters == null || _animator == null)
            return 60;
        if (!_animatorInitialized)
        {
            _animatorInitialized = true;
            var bools = 0;
            var floats = 0;
            var ints = 0;
            foreach (var parameter in parameters)
            {
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        bools++;
                        break;
                    case AnimatorControllerParameterType.Float:
                        floats++;
                        break;
                    case AnimatorControllerParameterType.Int:
                        ints++;
                        break;
                }
            }
            _lastBools = new bool[bools];
            _lastFloats = new float[floats];
            _lastInts = new int[ints];
        }
        var boolIndex = 0;
        var floatIndex = 0;
        var intIndex = 0;
        foreach (var parameter in parameters)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Bool:
                    var boolVal = _animator.GetBool(parameter.nameHash);
                    if (boolVal != _lastBools![boolIndex])
                    {
                        PacketManager.Send(new AnimateBoolPacket(
                            syncId,
                            parameter.nameHash,
                            boolVal
                        ), DeliveryMethod.Unreliable);
                    }
                    boolIndex++;
                    break;
                case AnimatorControllerParameterType.Float:
                    var floatVal = _animator.GetFloat(parameter.nameHash);
                    if(!Mathf.Approximately(floatVal, _lastFloats![floatIndex]))
                    {
                        PacketManager.Send(new AnimateFloatPacket(
                            syncId,
                            parameter.nameHash,
                            floatVal
                        ), DeliveryMethod.Unreliable);
                    }
                    floatIndex++;
                    break;
                case AnimatorControllerParameterType.Int:
                    var intVal = _animator.GetInteger(parameter.nameHash);
                    if(intVal != _lastInts![intIndex])
                    {
                        PacketManager.Send(new AnimateIntPacket(
                            syncId,
                            parameter.nameHash,
                            intVal
                        ), DeliveryMethod.Unreliable);
                    }
                    intIndex++;
                    break;
            }
        }
        return 4;
    }

    public void ReceiveAnimationBool(int id, bool val)
    {
        _animator?.SetBool(id, val);
    }

    public void ReceiveAnimationFloat(int id, float val)
    {
        _floatLerp?.SetFloat(id, val);
    }

    public void ReceiveAnimationInt(int id, int val)
    {
        _animator?.SetInteger(id, val);
    }
}