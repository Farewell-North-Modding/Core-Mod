using Il2CppKBCore.UI;
using Il2CppRTLTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace FarewellCore.GUI.Component;

/// <summary>
/// A generic class managing generic farewell mod ui methods
/// </summary>
[RegisterTypeInIl2Cpp]
public class FarewellLayout : MonoBehaviour
{
    /// <summary>
    /// Internal method that adds a component and retrieves and overwrites the RTLTextMeshPro component
    /// </summary>
    /// <param name="text">The text to overwrite with</param>
    /// <param name="type">The component registry type</param>
    /// <returns>The newly added text component (can be ignored)</returns>
    private RTLTextMeshPro AddText(string text, ComponentRegistry.ComponentType type)
    {
        var textObj = ComponentRegistry.CreateComponent(type);
        textObj.transform.SetParent(transform, false);
        var tmp = textObj.GetComponent<RTLTextMeshPro>();
        tmp.originalText = text;
        tmp.text = text;
        tmp.SetText(text);
        tmp.fontSizeMin = tmp.fontSizeMax;
        return tmp;
    }
    
    /// <summary>
    /// Adds a header to preferably a panel, just like the ones in the setting panels
    /// </summary>
    /// <param name="text">The text that gets put in the header</param>
    /// <returns>The newly added text component (can be ignored)</returns>
    public RTLTextMeshPro AddHeader(string text)
    {
        return AddText(text, ComponentRegistry.ComponentType.Header);
    }
    
    /// <summary>
    /// Adds a header to preferably a panel, just like the ones that describe settings in the setting panels
    /// </summary>
    /// <param name="text">The text that gets put in the label</param>
    /// <returns>The newly added text component (can be ignored)</returns>
    public RTLTextMeshPro AddLabel(string text)
    {
        return AddText(text, ComponentRegistry.ComponentType.Label);
    }

    /// <summary>
    /// Creates a panel with a see-through or fully black background
    /// </summary>
    /// <param name="solid">True if the background should be solid and not see-through</param>
    /// <returns>The FarewellGenericUI component of the new panel</returns>
    public FarewellLayout AddPanel(bool solid = false)
    {
        var panel = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.Panel);
        panel.transform.SetParent(transform, false);
        var generic = panel.AddComponent<FarewellLayout>();
        if (!solid)
            return generic;
        var img = panel.GetComponent<Image>();
        img.color = Color.black;
        return generic;
    }

    /// <summary>
    /// Adds a vertical layout group
    /// </summary>
    /// <returns>The FarewellLayout component of the new layout</returns>
    public FarewellLayout AddVerticalLayout()
    {
        var layout = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.VerticalLayout);
        layout.transform.SetParent(transform, false);
        return layout.AddComponent<FarewellLayout>();
    }

    /// <summary>
    /// Adds a horizontal layout group
    /// </summary>
    /// <returns>The FarewellLayout component of the new layout</returns>
    public FarewellLayout AddHorizontalLayout()
    {
        var layout = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.HorizontalLayout);
        layout.transform.SetParent(transform, false);
        return layout.AddComponent<FarewellLayout>();
    }

    /// <summary>
    /// Adds an on off toggle entry like in the settings
    /// </summary>
    /// <param name="label">The text on the left of the toggle. Is removed when not given.</param>
    /// <returns>Game-internal UIToggle element for further use</returns>
    public UIToggle AddToggleElement(string? label = null)
    {
        var toggle = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.Toggle);
        toggle.transform.SetParent(transform, false);
        var text = toggle.transform.GetChild(0).GetChild(0).GetComponent<RTLTextMeshPro>();
        if (label != null)
        {
            text.originalText = label;
            text.text = label;
            text.SetText(label);
            text.fontSizeMin = text.fontSizeMax;
        } else
            DestroyImmediate(text.gameObject);
        return toggle.transform.GetChild(0).GetChild(label == null ? 0 : 1).GetChild(0).GetComponent<UIToggle>();
    }
    
    /// <summary>
    /// Adds a custom object to the elements children
    /// </summary>
    /// <param name="obj">Custom object to add</param>
    public void AddCustom(GameObject obj)
    {
        obj.transform.SetParent(transform, false);
    }
}