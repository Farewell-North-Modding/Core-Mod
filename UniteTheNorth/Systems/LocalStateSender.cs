using FarewellCore.Tools;
using UnityEngine;

namespace UniteTheNorth.Systems;

public class LocalStateSender
{
    private static bool _initialized;
    private static bool[]? _lastBools;
    private static float[]? _lastFloats;
    private static int[]? _lastInts;

    public static void SendUpdates()
    {
        // Find Player
        var player = GameplayFinder.FindPlayer();
        if(player == null)
            return;
        
        // Send Animation State
        var animator = player.Animator;
        var parameters = animator.parameters;
        if(parameters == null || animator == null)
            return;
        if (!_initialized)
        {
            _initialized = true;
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