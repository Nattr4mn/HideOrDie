using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public UnityEvent KillEvent;

    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private float _playerMoveSpeed;

    private void Awake()
    {
        if( _playerRigidbody == null )
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
        }
    }

    public void Move(Vector2 axis)
    {
        Vector2 velocityVector = Vector2.zero;
        if (axis.y != 0 || axis.x != 0)
        {
            velocityVector = new Vector2(axis.x * _playerMoveSpeed, axis.y * _playerMoveSpeed);
            transform.eulerAngles = NewRotationVector(axis);
        }

        _playerRigidbody.velocity = velocityVector;
    }

    private Vector3 NewRotationVector(Vector2 axis)
    {
        var newRotation = transform.eulerAngles;
        if (axis.x != 0f)
            newRotation.z = (axis.x > 0f) ? -90f : 90f;
        else if (axis.y != 0f)
            newRotation.z = (axis.y > 0f) ? 0f : 180f;
        return newRotation;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public void Kill()
    {
        KillEvent?.Invoke();
    }
}
