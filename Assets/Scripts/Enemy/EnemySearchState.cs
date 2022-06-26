using UnityEngine;

public class EnemySearchState : EnemyMoveState
{
    private Vector3 _nextPosition;
    private Transform _targetTransform;

    public EnemySearchState(PathSearcher pathSearcher, Transform targetTransform) : base(pathSearcher)
    {
        _targetTransform = targetTransform;
    }

    public override Vector3 GetNextPosition(Vector3 currentPosition)
    {
        if(currentPosition == _nextPosition || _nextPosition == Vector3.zero)
        {
            var path = PathSearcher.TryGetNextPosition(currentPosition, _targetTransform.position);
            _nextPosition = new Vector3(path.x, path.y);
        }
        return _nextPosition;
    }
}
