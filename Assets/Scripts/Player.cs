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

    private void FixedUpdate()
    {
        var verticalAxis = Input.GetAxis("Vertical");
        var horizontalAxis = Input.GetAxis("Horizontal");
        Vector2 velocityVector = Vector2.zero;
        if (verticalAxis != 0 || horizontalAxis != 0)
        {
            velocityVector = new Vector2(horizontalAxis * _playerMoveSpeed, verticalAxis * _playerMoveSpeed);
        }
        _playerRigidbody.velocity = velocityVector;
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
