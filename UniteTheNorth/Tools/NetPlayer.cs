using MelonLoader;
using UnityEngine;

namespace UniteTheNorth.Tools;

[RegisterTypeInIl2Cpp]
public class NetPlayer : MonoBehaviour
{
    public float lerpSpeed = 10F;
    private Animator? _animator;
    private Vector3 _locationGoal;
    private Quaternion _rotationGoal;
    private AnimatorFloatLerp? _floatLerp;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Destroy(GetComponent<Rigidbody>());
        _floatLerp = new AnimatorFloatLerp(_animator);
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

    /*private Rigidbody? _rigidbody;
    private PlayerWrapper? _wrapper;
    public Vector3 moveToPosition;
    public Vector3 rotateToRotation;

    private void Start()
    {
        _wrapper = new PlayerWrapper(gameObject);
        moveToPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, moveToPosition) < .3F)
        {
            _wrapper?.Animal.RotateAtDirection(rotateToRotation, 5);
            return;
        }
        var direction = (moveToPosition - transform.position).normalized;
        _wrapper?.Animal.Move(direction);
        _rigidbody!.velocity = Vector3.zero;
    }

    public void UpdatePosition(Vector3 position)
    {
        moveToPosition = position;
    }

    public void UpdateRotation(Vector3 rotation)
    {
        rotateToRotation = rotation;
    }

    public void Jump()
    {
        _wrapper?.Jump();
    }*/
}