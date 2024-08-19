using FarewellCore.GUI.Impl;
using MelonLoader;

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
    }

    public override void OnUpdate()
    {
        RunOnNextUpdate.ForEach(action => action());
        RunOnNextUpdate.Clear();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        TitleScreenPatcher.Patch(sceneName);
    }
}