using Il2CppFarewellNorth.UI.Settings;
using Il2CppKBCore.Localization;
using Il2CppKBCore.Settings.UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace FarewellCore.GUI.Component;

/// <summary>
/// This is an internal registry that caches components for use in the farewell ui library
/// </summary>
public static class ComponentRegistry
{
    private static readonly Dictionary<ComponentType, GameObject> Components = new();
    private static readonly List<Action> AfterInit = new();
    private static bool _isInitializing;
    
    /// <summary>
    /// Initializes the component registry
    /// </summary>
    public static void Initialize()
    {
        _isInitializing = true;
        Components.Clear();
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Is called once a scene is loaded
    /// </summary>
    /// <param name="sceneName">The loaded scenes name</param>
    public static void OnSceneLoad(string sceneName)
    {
        if(sceneName != "Settings" || !_isInitializing)
            return;
        _isInitializing = false;
        try
        {
            // Retrieve scene and create cache canvas
            var scene = SceneManager.GetSceneByName("Settings");
            var cacheCanvas = new GameObject("FarewellCacheCanvas");
            cacheCanvas.AddComponent<Canvas>();
            Object.DontDestroyOnLoad(cacheCanvas);
            cacheCanvas.SetActive(false);
            var rootCanvas = scene.GetRootGameObjects().First();
            var accessibility = rootCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0);
            // Create Canvas Cache
            var canvas = Object.Instantiate(rootCanvas);
            Object.DestroyImmediate(canvas.GetComponent<SettingsController>());
            Object.DestroyImmediate(canvas.transform.GetChild(1).gameObject);
            Object.DestroyImmediate(canvas.transform.GetChild(0).gameObject);
            canvas.transform.name = "FarewellCanvas";
            Object.DontDestroyOnLoad(canvas);
            Components[ComponentType.Canvas] = canvas;
            // Create Vertical Layout Cache
            var vLayout = Object.Instantiate(rootCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject, cacheCanvas.transform);
            for (var i = 0; i < vLayout.transform.childCount; i++)
            {
                vLayout.transform.GetChild(i).SetParent(null);
                Object.Destroy(vLayout.transform.GetChild(i).gameObject);
            }
            vLayout.transform.name = "FarewellVerticalLayout";
            Components[ComponentType.VerticalLayout] = vLayout;
            // Create Horizontal Layout Cache
            var hLayout = Object.Instantiate(rootCanvas.transform.GetChild(0).gameObject, cacheCanvas.transform);
            Object.DestroyImmediate(hLayout.transform.GetChild(1).gameObject);
            Object.DestroyImmediate(hLayout.transform.GetChild(0).gameObject);
            hLayout.transform.name = "FarewellHorizontalLayout";
            Components[ComponentType.HorizontalLayout] = hLayout;
            // Create Panel Cache
            var panel = Object.Instantiate(accessibility.gameObject, cacheCanvas.transform);
            panel.SetActive(true);
            panel.transform.name = "FarewellPanel";
            Components[ComponentType.Panel] = panel;
            // Create Slider Cache
            var slider = panel.transform.GetChild(4);
            slider.SetParent(cacheCanvas.transform);
            slider.name = "FarewellSlider";
            Object.DestroyImmediate(slider.GetComponent<SettingsFloatValueUI>());
            Object.DestroyImmediate(slider.GetChild(0).GetChild(0).GetComponent<LocalizedTextMeshPro>());
            Components[ComponentType.Slider] = slider.gameObject;
            // Create Toggle Cache
            var toggle = panel.transform.GetChild(3);
            toggle.SetParent(cacheCanvas.transform);
            toggle.name = "FarewellToggle";
            Object.DestroyImmediate(toggle.GetComponent<SettingsBoolValueUI>());
            Object.DestroyImmediate(toggle.GetChild(0).GetChild(0).GetComponent<LocalizedTextMeshPro>());
            Components[ComponentType.Toggle] = toggle.gameObject;
            // Create Label Cache
            var unusedBool = panel.transform.GetChild(2);
            var label = unusedBool.GetChild(0).GetChild(0);
            label.SetParent(cacheCanvas.transform);
            label.name = "FarewellLabel";
            Object.DestroyImmediate(label.GetComponent<LocalizedTextMeshPro>());
            Object.DestroyImmediate(unusedBool.gameObject);
            Components[ComponentType.Label] = label.gameObject;
            // Create Dropdown Cache
            var dropdown = panel.transform.GetChild(1);
            dropdown.SetParent(cacheCanvas.transform);
            dropdown.name = "FarewellDropdown";
            Object.DestroyImmediate(dropdown.GetComponent<SettingsLanguageValueUI>());
            Object.DestroyImmediate(dropdown.GetChild(0).GetChild(0).GetComponent<LocalizedTextMeshPro>());
            Object.DestroyImmediate(dropdown.GetChild(0).GetChild(1).GetChild(0).GetComponent<LocalizedDropdown>());
            Components[ComponentType.Dropdown] = dropdown.gameObject;
            // Create Header Cache
            var header = panel.transform.GetChild(0);
            header.SetParent(cacheCanvas.transform);
            header.name = "FarewellHeader";
            Object.DestroyImmediate(header.GetComponent<LocalizedTextMeshPro>());
            Components[ComponentType.Header] = header.gameObject;
            // Finish Up
            foreach (var action in AfterInit)
                action();
            AfterInit.Clear();
            SceneManager.UnloadSceneAsync(scene);
        }
        catch (Exception e)
        {
            FarewellCore.Logger.Msg($"There was an issue while generating the component registry! ({e.Message})");
        }
    }

    /// <summary>
    /// Creates a game object from a cached component
    /// </summary>
    /// <param name="componentType">The type of component to create</param>
    /// <returns>The created game object</returns>
    public static GameObject CreateComponent(ComponentType componentType)
    {
        if (Components.TryGetValue(componentType, out var component))
            return Object.Instantiate(component);
        FarewellCore.Logger.Msg($"The registry is missing an entry for {componentType}!");
        return new GameObject("InvalidRegistryItem");
    }

    /// <summary>
    /// The different types of component that are cached in the registry
    /// </summary>
    public enum ComponentType
    {
        Canvas,
        Panel,
        VerticalLayout,
        HorizontalLayout,
        Header,
        Label,
        Toggle,
        Dropdown,
        Slider
    }

    /// <summary>
    /// Runs the passed action once the farewell ui lib is ready to be used. Only required on title screen scene load.
    /// </summary>
    /// <param name="action">The actual action to run</param>
    public static void RunOnReady(Action action)
    {
        if (Components.Count > 0)
            action();
        else
            AfterInit.Add(action);
    }
}