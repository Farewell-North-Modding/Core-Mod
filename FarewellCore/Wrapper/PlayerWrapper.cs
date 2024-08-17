using Il2CppFarewellNorth.Characters.Player;
using UnityEngine;

namespace FarewellCore.Wrapper;

public class PlayerWrapper
{
    public readonly PlayerInteraction PlayerInteraction;
    public readonly PlayerCharacter PlayerCharacter;

    public PlayerWrapper(GameObject gameObject)
    {
        PlayerInteraction = gameObject.GetComponent<PlayerInteraction>();
        PlayerCharacter = gameObject.GetComponent<PlayerCharacter>();
    }
}