using FarewellCore.Tools;
using UnityEngine;

namespace UniteTheNorth.Systems;

public static class LocalStateSender
{
    private static int _messageCooldown;
    private static bool _animatorInitialized;

    private static Vector3 _lastPosition = Vector3.zero;
    private static Quaternion _lastRotation = Quaternion.identity;
    private static bool[]? _lastBools;
    private static float[]? _lastFloats;
    private static int[]? _lastInts;

    public static void SendUpdates()
    {
        // Apply Cooldown
        _messageCooldown--;
        if(_messageCooldown > 0)
            return;
        _messageCooldown = 4;
        
        // Find Player
        var player = GameplayFinder.FindPlayer();
        if(player == null)
            return;
        
        // Send Position and Rotation
        var pTransform = player.GetGameObject().transform;
        if (Vector3.Distance(_lastPosition, pTransform.position) > .1F)
        {
            _lastPosition = pTransform.position;
            SendLocation(_lastPosition);
        }
        if (Quaternion.Angle(_lastRotation, pTransform.rotation) > 3F)
        {
            _lastRotation = pTransform.rotation;
            SendRotation(_lastRotation);
        }
        
        // Send Animation State
        var animator = player.Animator;
        var parameters = animator.parameters;
        if(parameters == null || animator == null)
            return;
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
                    var boolVal = animator.GetBool(parameter.nameHash);
                    if(boolVal != _lastBools![boolIndex])
                        SendAnimatorBool(parameter.nameHash, boolVal);
                    boolIndex++;
                    break;
                case AnimatorControllerParameterType.Float:
                    var floatVal = animator.GetFloat(parameter.nameHash);
                    if(!Mathf.Approximately(floatVal, _lastFloats![floatIndex]))
                        SendAnimatorFloat(parameter.nameHash, floatVal);
                    floatIndex++;
                    break;
                case AnimatorControllerParameterType.Int:
                    var intVal = animator.GetInteger(parameter.nameHash);
                    if(intVal != _lastInts![intIndex])
                        SendAnimatorInt(parameter.nameHash, intVal);
                    intIndex++;
                    break;
            }
        }
    }

    private static void SendLocation(Vector3 location)
    {
        UniteTheNorth._netPlayer?.ReceiveLocation(location + new Vector3(2, 0, 0));
    }

    private static void SendRotation(Quaternion rotation)
    {
        UniteTheNorth._netPlayer?.ReceiveRotation(rotation);
    }

    private static void SendAnimatorBool(int id, bool val)
    {
        UniteTheNorth._netPlayer?.ReceiveAnimationBool(id, val);
    }

    private static void SendAnimatorFloat(int id, float val)
    {
        UniteTheNorth._netPlayer?.ReceiveAnimationFloat(id, val);
    }

    private static void SendAnimatorInt(int id, int val)
    {
        UniteTheNorth._netPlayer?.ReceiveAnimationInt(id, val);
    }
}