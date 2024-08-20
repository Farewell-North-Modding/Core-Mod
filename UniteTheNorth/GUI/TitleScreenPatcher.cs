using MelonLoader;
using UnityEngine;
using UniverseLib.UI;

namespace UniteTheNorth.GUI;

[RegisterTypeInIl2Cpp]
public class TitleScreenPatcher : MonoBehaviour
{
    public static void Patch(string sceneName)
    {
        if(sceneName != "Main Menu")
            return;
        new GameObject("UTNTitleScreenPatcher").AddComponent<TitleScreenPatcher>();
    }

    private void Start()
    {
        ReplaceArtwork();
        Invoke(nameof(AddConnectUI), 2f);
        UniteTheNorth.Logger.Msg("Applied Title Screen Patches");
    }

    private static void ReplaceArtwork()
    {
        // TODO: Create Replacer
    }

    private void AddConnectUI()
    {
        //var uiBase = UniversalUI.RegisterUI("com.limo.UniteTheNorth", () => { });
        //var connectPanel = new ConnectPanel(uiBase);
    }
}