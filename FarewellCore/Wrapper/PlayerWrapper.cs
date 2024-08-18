using Il2CppFarewellNorth.Characters.Actions.Movement.Malbers;
using Il2CppFarewellNorth.Characters.Player;
using Il2CppMalbersAnimations.Controller;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FarewellCore.Wrapper;

public class PlayerWrapper
{
    private readonly GameObject _go;
    public readonly MalbersJumper Jumper;
    public readonly PlayerInteraction PlayerInteraction;
    public readonly PlayerCharacter PlayerCharacter;
    public readonly MAnimal Animal;
    public readonly Animator Animator;

    public PlayerWrapper(GameObject gameObject)
    {
        Jumper = gameObject.GetComponent<MalbersJumper>();
        PlayerInteraction = gameObject.GetComponent<PlayerInteraction>();
        PlayerCharacter = gameObject.GetComponent<PlayerCharacter>();
        Animal = gameObject.GetComponent<MAnimal>();
        Animator = gameObject.GetComponent<Animator>();
        _go = gameObject;
    }

    public GameObject GetGameObject()
    {
        return _go;
    }

    public PlayerWrapper CreatePlayerDummy()
    {
        var parent = GameObject.Find("DummyContainer");
        if (parent == null)
            parent = new GameObject("DummyContainer");
        var newCorePlayer = Object.Instantiate(_go, _go.transform.parent);
        _go.GetComponent<MalbersLocomotion>().enabled = false;
        Animal.enabled = false;
        _go.transform.SetParent(parent.transform);
        _go.transform.name = $"Player-Dummy-{Guid.NewGuid()}";
        newCorePlayer.transform.name = "Player";
        return this;
    }

    public void Jump()
    {
        Jumper.Jump(true);
    }
}