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
        AddConnectUI();
        UniteTheNorth.Logger.Msg("Applied Title Screen Patches");
    }

    private static void ReplaceArtwork()
    {
        // TODO: Create Artwork and create Replacer
    }

    private static void AddConnectUI()
    {
        UniverseLib.Universe.Init();
        var uiBase = UniversalUI.RegisterUI("com.limo.UniteTheNorth", () => { });
        var connectPanel = new ConnectPanel(uiBase);
    }
}