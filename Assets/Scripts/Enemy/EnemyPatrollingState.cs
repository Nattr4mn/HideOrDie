using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyState
{
    private List<Vector2> _patrollingPath;
    private Vector3 _nextPosition;
    private int _nextPositionIndex;

    public EnemyPatrollingState(PathSearcher pathSearcher, Vector2 startPoint, Vector2 endPoint):
        base(pathSearcher)
    {
        _patrollingPath = CreatePath(startPoint, endPoint);
        _nextPositionIndex = 0;
        _nextPosition = startPoint;
    }

    public override Vector3 GetNextPosition(Transform currentPosition)
    {
        if(currentPosition.position == _nextPosition)
        {
            if(_nextPositionIndex < _patrollingPath.Count - 1)
            {
                _nextPositionIndex++;
                _nextPosition = _patrollingPath[_nextPositionIndex];
            }
        }

        return _nextPosition;
    }
}
