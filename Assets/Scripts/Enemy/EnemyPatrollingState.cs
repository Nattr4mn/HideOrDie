using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyMoveState
{
    private List<Vector2> _patrollingPath;
    private Vector3 _nextPosition;
    private int _nextPositionIndex;
    private bool _isBack = false;

    public EnemyPatrollingState(PathSearcher pathSearcher, Vector2 startPoint, Vector2 endPoint):
        base(pathSearcher)
    {
        _patrollingPath = PathSearcher.CreatePath(startPoint, endPoint);
        _nextPositionIndex = 0;
        _nextPosition = startPoint;
    }

    public override Vector3 GetNextPosition(Vector3 currentPosition)
    {
        if(currentPosition == _nextPosition)
        {
            _nextPositionIndex = GetNextPositionIndex(_nextPositionIndex);
            _nextPosition = _patrollingPath[_nextPositionIndex];
        }

        return _nextPosition;
    }

    private int GetNextPositionIndex(int currentIndex)
    {
        if (currentIndex < _patrollingPath.Count - 1 && !_isBack)
        {
            currentIndex++;
        }
        else if (currentIndex > 0 && _isBack)
        {
            currentIndex--;
        }
        else
        {
            _isBack = !_isBack;
        }

        return currentIndex;
    }
}
