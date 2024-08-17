using FarewellCore.GUI;
using MelonLoader;

namespace FarewellCore;

public class FarewellCore : MelonMod
{
    public static MelonLogger.Instance? Logger { get; private set; }
    public const string Version = "0.1.0";

    public override void OnInitializeMelon()
    {
        Logger = LoggerInstance;
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        TitleScreenPatcher.Patch(sceneName);
    }
}