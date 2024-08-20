using FarewellCore.GUI.Component;

namespace FarewellCore.GUI;

public static class FarewellUI
{
    /// <summary>
    /// Creates a new canvas using the farewell mod ui lib
    /// </summary>
    /// <returns>The canvas to work with</returns>
    public static FarewellGenericUI CreateCanvas()
    {
        var go = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.Canvas);
        return go.AddComponent<FarewellGenericUI>();
    }
}