using UnityEngine;

public class EnemyMovement
{
    private float _enemySpeed;
    private float _rotationSpeed;
    private PathSearcher _pathSearcher;
    private EnemyMoveState _enemyState;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    public EnemyMovement(PathSearcher pathSearcher, Vector3 startPosition, Vector3 endPosition, float enemySpeed, float rotationSpeed)
    {
        _enemySpeed = enemySpeed;
        _rotationSpeed = rotationSpeed;
        _pathSearcher = pathSearcher;
        _startPosition = startPosition;
        _endPosition = endPosition;
        _enemyState = new EnemyPatrollingState(_pathSearcher, _startPosition, _endPosition);
    }

    public void SetSearchState(Transform target)
    {
        _enemyState = new EnemySearchState(_pathSearcher, target);
    }

    public void SetPatrollingState()
    {
        _enemyState = new EnemyPatrollingState(_pathSearcher, _startPosition, _endPosition);
    }

    public void Move(Transform enemyTransform)
    {
        var nextPosition = _enemyState.GetNextPosition(enemyTransform.position);
        nextPosition.z = enemyTransform.position.z;
        Rotate(enemyTransform, nextPosition);
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, nextPosition, _enemySpeed * Time.deltaTime);
    }

    private void Rotate(Transform enemyTransform, Vector3 nextPosition)
    {
        var difference = nextPosition - enemyTransform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;
        var currentRotation = enemyTransform.rotation;
        var targetRotation = Quaternion.Euler(0, 0, rotationZ);
        enemyTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
