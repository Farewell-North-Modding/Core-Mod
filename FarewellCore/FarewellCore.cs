using FarewellCore.GUI.Component;
using FarewellCore.GUI.Impl;
using MelonLoader;
using FarewellCore.Input;

namespace FarewellCore;

public class FarewellCore : FarewellMod
{
    /// <summary>
    /// All actions in this list will be run on the next non-fixed update. The List is cleared after updating. The List should never be modified in the containing actions.
    /// </summary>
    public static readonly List<Action> RunOnNextUpdate = new();
    
    public static MelonLogger.Instance Logger => Melon<FarewellCore>.Logger;

    public override void OnInitializeMelon()
    {
        ComponentRegistry.Initialize();
    }

    public override void OnUpdate()
    {
        InputHelper.UpdateCallback();
        RunOnNextUpdate.ForEach(action => action());
        RunOnNextUpdate.Clear();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        ComponentRegistry.OnSceneLoad(sceneName);
        TitleScreenPatcher.Patch(sceneName);
    }
}