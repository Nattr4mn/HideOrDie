using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private float _enemySpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private EnemyVision _enemyVision;
    [SerializeField] private SpriteRenderer _enemySpriteRenderer;
    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _aggressiveColor;
    private PathSearcher _pathSearcher;
    private EnemyMovement _movement;


    private void Start()
    {
        _enemyVision.SetAimDirection(transform.up.normalized);
        _enemyVision.DetectEvent.AddListener(SetSearchState);
    }

    public void SetSearchState(Transform target)
    {
        _movement.SetSearchState(target);
        _enemySpriteRenderer.color = _aggressiveColor;
    }

    private void Update()
    {
        _movement.Move(transform);
        _enemyVision.Scan(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.Kill();
        }
    }

    public void Init(MapBuilder mapBuilder, Vector2 startPosition, Vector2 endPosition)
    {
        _mapBuilder = mapBuilder;
        transform.position = startPosition;
        _enemySpriteRenderer.color = _neutralColor;
        _pathSearcher = new PathSearcher(_mapBuilder.Map);
        _movement = new EnemyMovement(_pathSearcher, startPosition, endPosition, _enemySpeed, _rotationSpeed);
    }
}
