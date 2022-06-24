using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{
    public UnityEvent FinishEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.gameObject.SetActive(false);
            FinishEvent?.Invoke();
        }
    }
}
