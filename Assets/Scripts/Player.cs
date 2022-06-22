using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private float _playerMoveSpeed;

    private void Awake()
    {
        if( _playerRigidbody == null )
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
        }
    }

    
}
