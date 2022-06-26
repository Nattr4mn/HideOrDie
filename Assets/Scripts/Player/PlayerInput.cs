using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void FixedUpdate()
    {
        var axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _player.Move(axis);
    }
}
