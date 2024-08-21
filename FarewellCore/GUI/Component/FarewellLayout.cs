using Il2CppKBCore.Localization;
using Il2CppKBCore.UI;
using Il2CppRTLTMPro;
using Il2CppTMPro;
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
    /// Adds a horizontal layout group
    /// </summary>
    /// <returns>The FarewellLayout component of the new layout</returns>
    public FarewellLayout AddLayout(bool horizontal = false)
    {
        var layout = new GameObject("FarewellHorizontalLayout");
        layout.transform.SetParent(transform, false);
        HorizontalOrVerticalLayoutGroup group = horizontal ? layout.AddComponent<HorizontalLayoutGroup>() : layout.AddComponent<VerticalLayoutGroup>();
        group.childAlignment = TextAnchor.UpperCenter;
        group.childControlWidth = true;
        group.childControlHeight = false;
        group.childForceExpandWidth = true;
        group.childForceExpandHeight = false;
        group.spacing = 20;
        var fit = layout.AddComponent<ContentSizeFitter>();
        fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fit.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        return layout.AddComponent<FarewellLayout>();
    }

    /// <summary>
    /// Internal method that adds a default element of no specific type to the layout
    /// </summary>
    /// <param name="label">The text on the left of the element. Is fully removed when not given.</param>
    /// <param name="type">The component type to pull with from the registry</param>
    /// <typeparam name="T">The type of the default element</typeparam>
    /// <returns>The default element according to the type param</returns>
    private T AddDefaultElement<T>(string? label, ComponentRegistry.ComponentType type)
    {
        var toggle = ComponentRegistry.CreateComponent(type);
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
        return toggle.transform.GetChild(0).GetChild(label == null ? 0 : 1).GetChild(0).GetComponent<T>();
    }

    /// <summary>
    /// Adds an on off toggle entry like in the settings
    /// </summary>
    /// <param name="label">The text on the left of the toggle. Is fully removed when not given.</param>
    /// <param name="defaultValue">The initial value of the toggle button</param>
    /// <param name="onValueChanged">Callback that is called once the value changes</param>
    /// <returns>Game-internal UIToggle element for further use</returns>
    public UIToggle AddToggle(string? label = null, bool defaultValue = false, Action<bool>? onValueChanged = null)
    {
        var toggle = AddDefaultElement<UIToggle>(label, ComponentRegistry.ComponentType.Toggle);
        toggle.SetValue(defaultValue);
        if(onValueChanged != null)
            toggle.OnValueChanged += onValueChanged;
        return toggle;
    }

    /// <summary>
    /// Adds a slider entry like in the settings
    /// </summary>
    /// <param name="min">The minimum value of the slider</param>
    /// <param name="max">The maximum value of the slider</param>
    /// <param name="value">The initial value of the slider</param>
    /// <param name="label">The text on the left of the slider. Is fully removed when not given.</param>
    /// <param name="onValueChanged">Callback that is called once the value changes</param>
    /// <returns>Game-internal UISlider element for further use</returns>
    public UISlider AddSlider(float min = 0, float max = 1, float value = 0, string? label = null, Action<float>? onValueChanged = null)
    {
        var slider = AddDefaultElement<UISlider>(label, ComponentRegistry.ComponentType.Slider);
        slider.SetRange(min, max);
        slider.SetValue(value);
        if(onValueChanged != null)
            slider.OnValueChanged += onValueChanged;
        return slider;
    }

    /// <summary>
    /// Adds a dropdown entry like in the settings
    /// </summary>
    /// <param name="entries"></param>
    /// <param name="defaultIndex"></param>
    /// <param name="label">The text on the left of the slider. Is fully removed when not given.</param>
    /// <param name="onValueChanged">Callback that is called once the value changes</param>
    /// <returns>Game-internal UISlider element for further use</returns>
    public UIDropdown AddDropdown(List<TMP_Dropdown.OptionData> entries, int defaultIndex = 0, string? label = null, Action<int>? onValueChanged = null)
    {
        if (entries.Count == 0)
        {
            FarewellCore.Logger.Warning("Creating dropdown with empty entries!");
        }
        var dropdown = AddDefaultElement<UIDropdown>(label, ComponentRegistry.ComponentType.Dropdown);
        dropdown._dropdown.options.Clear();
        foreach (var option in entries)
            dropdown._dropdown.options.Add(option);
        dropdown.SetValue(defaultIndex);
        if(onValueChanged != null)
            dropdown.OnValueChanged += onValueChanged;
        return dropdown;
    }

    /// <summary>
    /// Adds an input field based on the dropdown menus in the settings menu
    /// </summary>
    /// <param name="placeholder">The placeholder that is displayed when the text is empty</param>
    /// <param name="defaultValue">The default value that's in the input field on construction</param>
    /// <param name="label">The text on the left of the input field. Is fully removed when not given.</param>
    /// <param name="onValueChanged">Callback that is called once the value changes</param>
    /// <returns>Text mesh pro input field for further use</returns>
    public TMP_InputField AddInputField(string placeholder = "Enter Text...", string defaultValue = "", string? label = null, Action<string>? onValueChanged = null)
    {
        var dropdown = AddDefaultElement<UIDropdown>(label, ComponentRegistry.ComponentType.Dropdown);
        //transform.parent.parent.parent.name = "FarewellInput";
        DestroyImmediate(dropdown._dropdown);
        var dropdownTransform = dropdown.transform;
        DestroyImmediate(dropdown);
        var text = dropdownTransform.GetChild(0).GetComponent<RTLTextMeshPro>();
        var ph = Instantiate(text.gameObject, dropdownTransform).GetComponent<RTLTextMeshPro>();
        ph.transform.name = "Placeholder";
        ph.faceColor = new Color32(255, 255, 255, 160);
        ph.text = placeholder;
        var fi = dropdownTransform.gameObject.AddComponent<FarewellInput>();
        fi.defaultValue = defaultValue;
        var tmp = dropdownTransform.gameObject.AddComponent<TMP_InputField>();
        tmp.onValueChanged.AddListener(onValueChanged);
        return tmp;
    }

    /// <summary>
    /// Adds a custom button based on the dropdown menus in the settings menu
    /// </summary>
    /// <param name="buttonText">The text that should be displayed on the button</param>
    /// <param name="onClick">Callback that gets called on button click</param>
    /// <returns>Game-internal UIButton element for further use</returns>
    public UIButton AddBoxButton(string buttonText, Action? onClick = null)
    {
        var toggle = AddDefaultElement<UIToggle>(null, ComponentRegistry.ComponentType.Toggle);
        var toggleTransform = toggle.transform;
        DestroyImmediate(toggle);
        var ub = toggleTransform.gameObject.AddComponent<UIButton>();
        toggleTransform.gameObject.AddComponent<Image>();
        DestroyImmediate(toggleTransform.GetChild(1).gameObject);
        var btn = toggleTransform.GetComponent<Button>();
        if(onClick != null)
            btn.onClick.AddListener(onClick);
        var txtComp = toggleTransform.GetChild(0).gameObject;
        DestroyImmediate(txtComp.GetComponent<LocalizedTextMeshPro>());
        var txt = txtComp.GetComponent<RTLTextMeshPro>();
        txt.text = buttonText;
        txt.faceColor = new Color32(10, 10, 10, 255);
        return ub;
    }

    /// <summary>
    /// Adds a button based on the buttons in the settings menu
    /// </summary>
    /// <param name="buttonText">The text that should be displayed on the button</param>
    /// <param name="onClick">Callback that gets called on button click</param>
    /// <returns>Game-internal UIButton element for further use</returns>
    public UIButton AddButton(string buttonText, Action? onClick = null)
    {
        var button = ComponentRegistry.CreateComponent(ComponentRegistry.ComponentType.Button);
        button.transform.SetParent(transform);
        var text = button.transform.GetChild(1).GetComponent<RTLTextMeshPro>();
        text.originalText = buttonText;
        text.text = buttonText;
        text.SetText(buttonText);
        text.fontSizeMin = text.fontSizeMax;
        var uiButton = button.GetComponent<UIButton>();
        if(onClick != null)
            uiButton._button.onClick.AddListener(onClick);
        return uiButton;
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