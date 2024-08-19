using Il2CppKBCore.Persistence;
using Il2CppMalbersAnimations.Controller;
using Il2CppTMPro;
using MelonLoader;
using UniteTheNorth.Networking.Behaviour;
using UnityEngine;

namespace UniteTheNorth.Tools;

[RegisterTypeInIl2Cpp]
public class NetPlayer : MonoBehaviour
{
    private string? _username;
    private TextMeshPro? _text;
    private Camera? _camera;

    private void Start()
    {
        _camera = Camera.main;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<MAnimal>());
        Destroy(GetComponent<UniqueId>());
        var syncId = int.Parse(transform.name.Split('-')[1]);
        gameObject.AddComponent<NetworkAnimator>().OverwriteSyncId(syncId);
        gameObject.AddComponent<NetworkPosition>().OverwriteSyncId(syncId);
        gameObject.AddComponent<NetworkRotation>().OverwriteSyncId(syncId);
        var textObject = new GameObject("NameTag")
        {
            transform = { parent = transform }
        };
        _text = textObject.AddComponent<TextMeshPro>();
        _text.text = _username ?? "NameTag";
        _text.fontSize = 1.6F;
        _text.color = new Color(220, 220, 220);
        textObject.transform.localPosition = new Vector3(0, 0.8F, 0);
        _text.verticalAlignment = VerticalAlignmentOptions.Middle;
        _text.horizontalAlignment = HorizontalAlignmentOptions.Center;
    }

    private void Update()
    {
        if (_camera is null) return;
        _text?.transform.LookAt(_camera.transform);
        _text?.transform.Rotate(0, 180, 0);
    }

    public void ReceivePlayerInfo(string username)
    {
        _username = username;
        if (_text != null)
            _text.text = username;
    }
}