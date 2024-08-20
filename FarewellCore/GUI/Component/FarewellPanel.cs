using Il2CppRTLTMPro;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace FarewellCore.GUI.Component;

[RegisterTypeInIl2Cpp]
public class FarewellPanel : MonoBehaviour
{
    public string? header;
    public RTLTextMeshPro? headerElement;
    
    private void Start()
    {
        gameObject.AddComponent<VerticalLayoutGroup>().childScaleHeight = true;
        gameObject.AddComponent<ContentSizeFitter>();
        if(header == null || headerElement == null)
            return;
        headerElement.originalText = header;
        headerElement.text = header;
        headerElement.SetText(header);
        headerElement.color = new Color(0.6604F, 0.6604F, 0.6604F);
        headerElement.alignment = TextAlignmentOptions.Top;
    }

    private void Update()
    {
        if (headerElement is not null) headerElement.transform.localPosition = new Vector3(0, -30, 0);
    }

    public static FarewellPanel Create(Transform parent, string? header = null, bool solidBackground = false)
    {
        var panel = new GameObject("FarewellPanel");
        var ui = panel.AddComponent<FarewellPanel>();
        ui.header = header;
        panel.transform.SetParent(parent);
        panel.AddComponent<Image>().color = solidBackground ? Color.black : new Color(0, 0, 0, 0.9804F);
        panel.GetComponent<RectTransform>().localScale = new Vector3(.75F, .75F, .75F);
        if (header == null) return ui;
        var headerElement = new GameObject("FarewellPanelHeader");
        headerElement.transform.SetParent(ui.transform);
        ui.headerElement = headerElement.AddComponent<RTLTextMeshPro>();
        return ui;
    }
}