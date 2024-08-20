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
    private static bool isInitializing;
    
    /// <summary>
    /// Initializes the component registry
    /// </summary>
    public static void Initialize()
    {
        isInitializing = true;
        Components.Clear();
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Is called once a scene is loaded
    /// </summary>
    /// <param name="sceneName">The loaded scenes name</param>
    public static void OnSceneLoad(string sceneName)
    {
        if(sceneName != "Settings" || !isInitializing)
            return;
        isInitializing = false;
        try
        {
            // Retrieve scene and create cache canvas
            var scene = SceneManager.GetSceneByName("Settings");
            var cacheCanvas = new GameObject("FarewellCacheCanvas");
            cacheCanvas.AddComponent<Canvas>();
            Object.DontDestroyOnLoad(cacheCanvas);
            cacheCanvas.SetActive(false);
            // Try to get the "accessibility" game object for components
            var accessibility = scene.GetRootGameObjects()[0].transform
                .GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0);
            // Create Panel Cache
            var panel = Object.Instantiate(accessibility.gameObject, cacheCanvas.transform);
            panel.SetActive(true);
            panel.transform.name = "FarewellPanel";
            Components[ComponentType.Panel] = panel;
            // Create Slider Cache
            var slider = panel.transform.GetChild(4);
            slider.SetParent(cacheCanvas.transform);
            slider.name = "FarewellSlider";
            Components[ComponentType.Slider] = slider.gameObject;
            // Create Toggle Cache
            var toggle = panel.transform.GetChild(3);
            toggle.SetParent(cacheCanvas.transform);
            toggle.name = "FarewellToggle";
            Components[ComponentType.Toggle] = toggle.gameObject;
            // Create Label Cache
            var unusedBool = panel.transform.GetChild(2);
            var label = unusedBool.GetChild(0).GetChild(0);
            label.SetParent(cacheCanvas.transform);
            label.name = "FarewellLabel";
            Object.Destroy(unusedBool.gameObject);
            Components[ComponentType.Label] = label.gameObject;
            // Create Enum Cache
            var enumSelect = panel.transform.GetChild(1);
            enumSelect.SetParent(cacheCanvas.transform);
            enumSelect.name = "FarewellEnumSelect";
            Components[ComponentType.Enum] = enumSelect.gameObject;
            // Create Header Cache
            var header = panel.transform.GetChild(0);
            header.SetParent(cacheCanvas.transform);
            header.name = "FarewellHeader";
            Components[ComponentType.Header] = header.gameObject;
            // Clean Up
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
        if (!Components.ContainsKey(componentType))
        {
            FarewellCore.Logger.Msg($"The registry is missing an entry for {componentType}!");
            return new GameObject("Invalid");
        }
        return Object.Instantiate(Components[componentType]);
    }

    /// <summary>
    /// The different types of component that are cached in the registry
    /// </summary>
    public enum ComponentType
    {
        Panel,
        Header,
        Label,
        Toggle,
        Enum,
        Slider
    }
}