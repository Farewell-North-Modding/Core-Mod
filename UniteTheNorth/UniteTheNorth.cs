using MelonLoader;

namespace UniteTheNorth;

public class UniteTheNorth : MelonMod
{
    public static MelonLogger.Instance? Logger { get; private set; }
    public const string Version = "0.1.0";

    public override void OnInitializeMelon()
    {
        Logger = LoggerInstance;
    }
}