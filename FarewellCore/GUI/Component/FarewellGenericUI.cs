using Il2CppRTLTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace FarewellCore.GUI.Component;

/// <summary>
/// A generic class managing generic farewell mod ui methods
/// </summary>
[RegisterTypeInIl2Cpp]
public class FarewellGenericUI : MonoBehaviour
{
    /// <summary>
    /// Internal method that adds a component and retrieves and overwrites the RTLTextMeshPro component
    /// </summary>
    /// <param name="text">The text to overwrite with</param>
    /// <param name="type">The component registry type</param>
    /// <returns></returns>
    private RTLTextMeshPro AddText(string text, ComponentRegistry.ComponentType type)
    {
        var textObj = ComponentRegistry.CreateComponent(type);
        textObj.transform.SetParent(transform, false);
        var tmp = textObj.GetComponent<RTLTextMeshPro>();
        tmp.SetText(text);
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
    /// Creates a panel with a see-through or fully black background
    /// </summary>
    /// <param name="solid">True if the background should be solid and not see-through</param>
    /// <returns>The FarewellGeneric component of the new panel</returns>
    public FarewellGenericUI AddPanel(bool solid = false)
    {
        var panel = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.Panel);
        panel.transform.SetParent(transform, false);
        var generic = panel.AddComponent<FarewellGenericUI>();
        if (!solid)
            return generic;
        var img = panel.GetComponent<Image>();
        img.color = Color.black;
        return generic;
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
    /// Adds a custom object to the elements children
    /// </summary>
    /// <param name="obj">Custom object to add</param>
    public void AddCustom(GameObject obj)
    {
        obj.transform.SetParent(transform, false);
    }
}