using FarewellCore.Wrapper;
using MelonLoader;
using UnityEngine;

namespace UniteTheNorth.Tools;

[RegisterTypeInIl2Cpp]
public class NetPlayer : MonoBehaviour
{
    private Animator? _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ReceiveAnimation(int id)
    {
        _animator?.Play(id);
    }

    public void ReceiveAnimationBool(int id, bool val)
    {
        _animator?.SetBool(id, val);
    }

    public void ReceiveAnimationFloat(int id, float val)
    {
        _animator?.SetFloat(id, val);
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