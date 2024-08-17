using Il2CppRTLTMPro;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace FarewellCore.GUI;

public static class TitleScreenPatcher
{
    public static void Patch(string sceneName)
    {
        if(sceneName != "Main Menu")
            return;
        PatchVersionNumber();
        PatchMenuButton();
    }

    private static void PatchMenuButton()
    {
        var settingsButton = GameObject.Find("Settings");
        var modsButton = Object.Instantiate(settingsButton, settingsButton.transform.parent);
        modsButton.transform.SetSiblingIndex(3);
        var clickedEvent = modsButton.GetComponent<Button>().onClick;
        clickedEvent.RemoveAllListeners();
        // Add the new clickevent
        var label = modsButton.GetComponentInChildren<RTLTextMeshPro>();
        label.text = "Mods";
        label.color = new Color(180, 180, 180);
    }

    private static void ModsButtonClickEvent()
    {
        FarewellCore.Logger?.Msg("Mods!");
    }

    private static void PatchVersionNumber()
    {
        var obj = GameObject.Find("Version Label");
        var txt = obj.GetComponent<TextMeshProUGUI>();
        var newText = $"Game ${txt.text} by Kyle Banks\nCore Mod v{FarewellCore.Version} by Limo";
        newText = MelonMod.RegisteredMelons
            .Where(mod => mod.Info.Name != "FarewellCore")
            .Aggregate(newText, (current, mod) => 
                current + $"\n{mod.Info.Name} v{mod.Info.Version} by {mod.Info.Author}");
        txt.text = newText;
    }
}