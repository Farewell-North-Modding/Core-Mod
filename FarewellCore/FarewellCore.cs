using FarewellCore.GUI;
using MelonLoader;

namespace FarewellCore;

public class FarewellCore : MelonMod
{
    public static MelonLogger.Instance Logger { get { return Melon<FarewellCore>.Logger; } }
    public const string Version = "0.1.0";

    public override void OnInitializeMelon()
    {
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        TitleScreenPatcher.Patch(sceneName);
    }
}