using Il2CppMalbersAnimations.Controller;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

namespace UniteTheNorth.Tools;

[RegisterTypeInIl2Cpp]
public class NetPlayer : MonoBehaviour
{
    public float lerpSpeed = 5F;
    private Animator? _animator;
    private Vector3 _locationGoal;
    private Quaternion _rotationGoal;
    private AnimatorFloatLerp? _floatLerp;
    private string? _username;
    private TextMeshPro? _text;

    private void Start()
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<MAnimal>());
        _animator = GetComponent<Animator>();
        _floatLerp = new AnimatorFloatLerp(_animator);
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
        _floatLerp?.Update();
        if (Vector3.Distance(_locationGoal, transform.position) > .1F)
        {
            transform.position = Vector3.Lerp(transform.position, _locationGoal, lerpSpeed * Time.deltaTime);
        }
        if (Quaternion.Angle(_rotationGoal, transform.rotation) > 3F)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _rotationGoal, lerpSpeed * Time.deltaTime);
        }
        if (Camera.main == null) return;
        _text?.transform.LookAt(Camera.main.transform);
        _text?.transform.Rotate(0, 180, 0);
    }

    public void ReceivePlayerInfo(string username)
    {
        _username = username;
        if (_text != null)
            _text.text = username;
    }

    public void ReceiveLocation(Vector3 location)
    {
        _locationGoal = location;
    }

    public void ReceiveRotation(Quaternion rotation)
    {
        _rotationGoal = rotation;
    }

    public void ReceiveAnimationBool(int id, bool val)
    {
        _animator?.SetBool(id, val);
    }

    public void ReceiveAnimationFloat(int id, float val)
    {
        _floatLerp?.SetFloat(id, val);
    }

    public void ReceiveAnimationInt(int id, int val)
    {
        _animator?.SetInteger(id, val);
    }
}