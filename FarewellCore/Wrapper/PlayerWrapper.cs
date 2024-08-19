using Il2CppFarewellNorth.Characters.Actions.Movement.Malbers;
using Il2CppFarewellNorth.Characters.Actions.Petting;
using Il2CppFarewellNorth.Characters.Actions.Petting.Logic;
using Il2CppFarewellNorth.Characters.Player;
using Il2CppFarewellNorth.Vehicles;
using Il2CppInterop.Runtime;
using Il2CppKBCore.Persistence;
using Il2CppMalbersAnimations.Controller;
using UnityEngine;
using Object = UnityEngine.Object;
using Type = Il2CppSystem.Type;

namespace FarewellCore.Wrapper;

public class PlayerWrapper
{
    private readonly GameObject _go;
    public readonly MalbersJumper Jumper;
    public readonly PlayerInteraction PlayerInteraction;
    public readonly PlayerCharacter PlayerCharacter;
    public readonly MAnimal Animal;
    public readonly Animator Animator;

    /// <summary>
    /// Creates a wrapper around the Player GameObject
    /// </summary>
    /// <param name="gameObject">The GameObject to wrap</param>
    public PlayerWrapper(GameObject gameObject)
    {
        Jumper = gameObject.GetComponent<MalbersJumper>();
        PlayerInteraction = gameObject.GetComponent<PlayerInteraction>();
        PlayerCharacter = gameObject.GetComponent<PlayerCharacter>();
        Animal = gameObject.GetComponent<MAnimal>();
        Animator = gameObject.GetComponent<Animator>();
        _go = gameObject;
    }

    /// <summary>
    /// Returns the players GameObject
    /// </summary>
    /// <returns>Players GameObject</returns>
    public GameObject GetGameObject()
    {
        return _go;
    }

    /// <summary>
    /// Creates a non-playable copy of the Chesley
    /// </summary>
    /// <returns>A new PlayerWrapper instance wrapping the dummy</returns>
    public PlayerWrapper CreatePlayerDummy()
    {
        // Generate Parent
        var parent = GameObject.Find("DummyContainer");
        if (parent == null)
            parent = new GameObject("DummyContainer");
        // Create modified Object
        var playerDummy = Object.Instantiate(_go, parent.transform);
        playerDummy.transform.name = $"Player-Dummy-{Guid.NewGuid()}";
        playerDummy.GetComponent<MalbersLocomotion>().enabled = false;
        playerDummy.GetComponent<PlayerInteraction>().enabled = false;
        playerDummy.GetComponent<VehicleDeactivatesGameObject>().enabled = false;
        playerDummy.GetComponent<UniqueId>().enabled = false;
        playerDummy.GetComponent<Persister>().enabled = false;
        // Register state restore and return
        FarewellCore.RunOnNextUpdate.Add(() =>
        {
            PlayerCharacter.Start();
            foreach (var logicalPettable in Resources.FindObjectsOfTypeAll<LogicalPettable>())
                logicalPettable.SetValue(_go.GetComponent<Pettable>());
        });
        return new PlayerWrapper(playerDummy);
    }
}