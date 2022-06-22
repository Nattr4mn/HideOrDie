using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private Transform _playerTransform;
    private WaySearcher _waySearcher;
    private Vector3 _currentTargetPoint;

    private void Start()
    {
        _waySearcher = new WaySearcher(_mapBuilder.Map);
        Way();
    }

    private void Update()
    {
        Way();
        if(_currentTargetPoint != Vector3.zero)
            transform.position = Vector3.MoveTowards(transform.position, _currentTargetPoint, 1f * Time.deltaTime);
        print(_currentTargetPoint);
    }

    private void Way()
    {
        if(transform.position == _currentTargetPoint || _currentTargetPoint == Vector3.zero)
        {
            var currentVector = new Vector2Int((int)transform.position.x, (int)transform.position.y);

            var targetVectorX = (_playerTransform.position.x - (int)_playerTransform.position.x >= 0.5f) ? Mathf.CeilToInt(_playerTransform.position.x) : (int)_playerTransform.position.x;
            var targetVectorY = (_playerTransform.position.y - (int)_playerTransform.position.y >= 0.5f) ? Mathf.CeilToInt(_playerTransform.position.y) : (int)_playerTransform.position.y;
            var targetVector = new Vector2Int(targetVectorX, targetVectorY);
            var nextPosition = _waySearcher.SearchWay(currentVector, targetVector);
            _currentTargetPoint = new Vector3(nextPosition[2].x, nextPosition[2].y);
        }
    }
}
