using FarewellCore.GUI.Component;
using Il2CppKBCore.Localization;
using Il2CppRTLTMPro;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace FarewellCore.GUI.Impl;

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
        /* UI Test
        ComponentRegistry.RunOnReady(() =>
        {
            var baseComp = GameObject.Find("Container").transform;
            var baseLayout = FarewellUI.CreateHorizontalLayout(baseComp);
            baseComp.GetChild(0).transform.SetParent(baseLayout.transform, false);
            baseLayout.transform.SetSiblingIndex(0);
            var panel = FarewellUI.CreatePanel(baseLayout.transform);
            panel.AddHeader("This is a test UI!");
            panel.AddLabel("This is a test Label!");
            var layout = panel.AddHorizontalLayout();
            layout.AddLabel("This is a text in");
            var vLayout = layout.AddVerticalLayout();
            vLayout.AddLabel("a horizontal layout with a");
            vLayout.AddLabel("vertical layout inside!");
            panel.AddLabel("This is a toggle without a label!");
            panel.AddToggle().SetValue(true);
            var toggle = panel.AddToggle("This is a toggle with a label!");
            toggle.SetValue(false);
            toggle.add_OnValueChanged(new Action<bool>(val =>
            {
                FarewellCore.Logger.Msg($"Toggled: {val}");
            }));
            var slider = panel.AddSlider(label: "This is a slider!");
            slider.add_OnValueChanged(new Action<float>(val =>
            {
                FarewellCore.Logger.Msg($"Slider: {val}");
            }));
            var dropdown = panel.AddDropdown(new List<TMP_Dropdown.OptionData>()
            {
                new ("Test 1"),
                new ("Test 2"),
                new ("Test 3"),
                new ("Test 4")
            }, 1, "This is a dropdown!");
            dropdown.add_OnValueChanged(new Action<int>(val =>
            {
                FarewellCore.Logger.Msg($"Dropdown: {val}");
            }));
        });*/
        FarewellCore.Logger.Msg("Applied Title Screen Patches");
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
        var newText = $"Game {txt.text} by Kyle Banks\nCore Mod v{Melon<FarewellCore>.Instance.Info.Version} by Limo";
        newText = MelonMod.RegisteredMelons
            .Where(mod => mod.Info.Name != "FarewellCore")
            .Aggregate(newText, (current, mod) => 
                current + $"\n{mod.Info.Name} v{mod.Info.Version} by {mod.Info.Author}");
        txt.SetText(newText);
    }
}