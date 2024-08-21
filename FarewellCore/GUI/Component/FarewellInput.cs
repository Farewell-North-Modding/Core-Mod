using Il2CppRTLTMPro;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

namespace FarewellCore.GUI.Component;

[RegisterTypeInIl2Cpp]
public class FarewellInput : MonoBehaviour
{
    public string? defaultValue;

    private void Start()
    {
        var field = GetComponent<TMP_InputField>();
        field.placeholder = transform.GetChild(2).GetComponent<RTLTextMeshPro>();
        field.textComponent = transform.GetChild(0).GetComponent<RTLTextMeshPro>();
        field.text = defaultValue ?? "";
    }
}