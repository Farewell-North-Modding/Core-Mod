using FarewellCore.GUI.Component;

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
}