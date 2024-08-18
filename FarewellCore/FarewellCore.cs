using FarewellCore.GUI;
using MelonLoader;

namespace FarewellCore;

public class FarewellCore : FarewellMod
{
    public static MelonLogger.Instance Logger { get { return Melon<FarewellCore>.Logger; } }

    public override void OnInitializeMelon()
    {
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        TitleScreenPatcher.Patch(sceneName);
    }
}