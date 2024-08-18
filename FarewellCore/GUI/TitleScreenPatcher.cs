using Il2CppKBCore.Localization;
using Il2CppRTLTMPro;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace FarewellCore.GUI;

[RegisterTypeInIl2Cpp]
public class TitleScreenPatcher : MonoBehaviour
{
    public static void Patch(string sceneName)
    {
        if(sceneName != "Main Menu")
            return;
        new GameObject("TitleScreenPatcher").AddComponent<TitleScreenPatcher>();
    }

    private void Start()
    {
        PatchVersionNumber();
        PatchMenuButton();
        FarewellCore.Logger?.Msg("Applied Title Screen Patches");
    }

    private static void PatchMenuButton()
    {
        var settingsButton = GameObject.Find("Settings");
        var modsButton = Instantiate(settingsButton, settingsButton.transform.parent);
        modsButton.transform.name = "Mods";
        modsButton.transform.SetSiblingIndex(3);
        modsButton.GetComponent<Button>().interactable = false;
        modsButton.GetComponentInChildren<LocalizedTextMeshPro>().enabled = false;
        var label = modsButton.GetComponentInChildren<RTLTextMeshPro>();
        label.originalText = "Mods";
        label.text = "Mods";
        label.SetText("Mods");
    }

    private static void PatchVersionNumber()
    {
        var obj = GameObject.Find("Version Label");
        var txt = obj.GetComponent<TextMeshProUGUI>();
        var newText = $"Game {txt.text} by Kyle Banks\nCore Mod v{FarewellCore.Version} by Limo";
        newText = MelonMod.RegisteredMelons
            .Where(mod => mod.Info.Name != "FarewellCore")
            .Aggregate(newText, (current, mod) => 
                current + $"\n{mod.Info.Name} v{mod.Info.Version} by {mod.Info.Author}");
        txt.SetText(newText);
    }
}