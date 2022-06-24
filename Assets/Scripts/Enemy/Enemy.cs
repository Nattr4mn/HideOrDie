using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private float _enemySpeed;
    [SerializeField] private SpriteRenderer _enemySpriteRenderer;
    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _aggressiveColor;
    [SerializeField] private EnemyVision _enemyVision;
    private PathSearcher _pathSearcher;
    private EnemyState _enemyState;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private void Start()
    {
        _pathSearcher = new PathSearcher(_mapBuilder.Map);
        _enemySpriteRenderer.color = _neutralColor;
        _enemyState = new EnemyPatrollingState(_pathSearcher, _startPosition, _endPosition);
        _enemyVision.SetAimDirection(transform.up.normalized);
        _enemyVision.DetectEvent.AddListener(SetSearchState);
    }

    private void Update()
    {
        var nextPosition = _enemyState.GetNextPosition(transform);
        nextPosition.z = transform.position.z;
        Rotate(nextPosition);
        _enemyVision.SetOrigin(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, _enemySpeed * Time.deltaTime);
    }

    private void Rotate(Vector3 nextPosition)
    {
        var difference = nextPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationZ), 10f * Time.deltaTime);
    }

    public void Init(MapBuilder mapBuilder, Vector2 startPosition, Vector2 endPosition)
    {
        _mapBuilder = mapBuilder;
        _startPosition = startPosition;
        _endPosition = endPosition;
        transform.position = _startPosition;
    }

    public void SetSearchState(Transform target)
    {
        _enemyState = new EnemySearchState(_pathSearcher, target);
        _enemySpriteRenderer.color = _aggressiveColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.Kill();
        }
    }
}
