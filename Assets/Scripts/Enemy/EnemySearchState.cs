using UnityEngine;

public class EnemySearchState : EnemyState
{
    private Vector3 _nextPosition;
    private Transform _targetTransform;

    public EnemySearchState(PathSearcher pathSearcher, Transform targetTransform) : base(pathSearcher)
    {
        _targetTransform = targetTransform;
    }

    public override Vector3 GetNextPosition(Transform currentPosition)
    {
        if (currentPosition.position == _nextPosition || _nextPosition == Vector3.zero)
        {
            var path = CreatePath(currentPosition.position, _targetTransform.position);
            _nextPosition = new Vector3(path[2].x, path[2].y);
        }
        return _nextPosition;
    }
}
