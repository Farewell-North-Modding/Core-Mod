using FarewellCore.GUI.Component;
using UnityEngine;
using UnityEngine.UI;

namespace FarewellCore.GUI;

public static class FarewellUI
{
    /// <summary>
    /// Creates a new canvas once the component library is ready and builds a ui using the callback
    /// </summary>
    /// <param name="buildUI">The function that builds a UI</param>
    public static void CreateFarewellUI(Action<FarewellLayout> buildUI)
    {
        ComponentRegistry.RunOnReady(() =>
        {
            buildUI.Invoke(CreateCanvas());
        });
    }
    
    /// <summary>
    /// Creates a new canvas using the farewell mod ui lib
    /// </summary>
    /// <returns>The canvas to work with</returns>
    public static FarewellLayout CreateCanvas()
    {
        var go = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.Canvas);
        return go.AddComponent<FarewellLayout>();
    }

    public static FarewellLayout CreatePanel(Transform parent, bool solid = false)
    {
        var panel = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.Panel);
        panel.transform.SetParent(parent, false);
        var generic = panel.AddComponent<FarewellLayout>();
        if (!solid)
            return generic;
        var img = panel.GetComponent<Image>();
        img.color = Color.black;
        return generic;
    }
    
    public static FarewellLayout CreateHorizontalLayout(Transform parent)
    {
        var layout = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.HorizontalLayout);
        layout.transform.SetParent(parent, false);
        return layout.AddComponent<FarewellLayout>();
    }
}