using FarewellCore.Wrapper;
using UnityEngine;

namespace FarewellCore.Tools;

public static class GameplayFinder
{
    public static PlayerWrapper? FindPlayer()
    {
        var obj = GameObject.Find("Player");
        return obj == null ? null : new PlayerWrapper(obj);
    }
}