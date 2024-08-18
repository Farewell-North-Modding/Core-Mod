using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

namespace UniteTheNorth.Tools;

[RegisterTypeInIl2Cpp]
public class NetPlayer : MonoBehaviour
{
    public float lerpSpeed = 6F;
    private Animator? _animator;
    private Vector3 _locationGoal;
    private Quaternion _rotationGoal;
    private AnimatorFloatLerp? _floatLerp;
    public TextMeshPro? text;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Destroy(GetComponent<Rigidbody>());
        _floatLerp = new AnimatorFloatLerp(_animator);
        var textObject = new GameObject("NameTag")
        {
            transform = { parent = transform }
        };
        text = textObject.AddComponent<TextMeshPro>();
        text.text = "NameTag";
        text.fontSize = 1.6F;
        text.color = Color.white;
        textObject.transform.localPosition = new Vector3(0, 0.8F, 0);
        text.verticalAlignment = VerticalAlignmentOptions.Middle;
        text.horizontalAlignment = HorizontalAlignmentOptions.Center;
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
        text?.transform.LookAt(Camera.main.transform);
        text?.transform.Rotate(0, 180, 0);
    }

    public void ReceivePlayerInfo(string username)
    {
        if (text != null)
            text.text = username;
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