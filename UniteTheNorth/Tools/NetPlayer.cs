using FarewellCore.Wrapper;
using MelonLoader;
using UnityEngine;

namespace UniteTheNorth.Tools;

[RegisterTypeInIl2Cpp]
public class NetPlayer : MonoBehaviour
{
    private PlayerWrapper? _wrapper;
    public Vector3 moveToPosition;
    public float speed = 5F;

    private void Start()
    {
        _wrapper = new PlayerWrapper(gameObject);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, moveToPosition) < .5F)
        {
            _wrapper?.Animal.StopMoving();
            return;
        }
        var direction = (moveToPosition - transform.position).normalized;
        _wrapper?.Animal.Move(direction);
        transform.position = Vector3.Lerp(transform.position, moveToPosition, speed * Time.deltaTime);
    }

    public void UpdatePosition(Vector3 position)
    {
        moveToPosition = position;
    }
}