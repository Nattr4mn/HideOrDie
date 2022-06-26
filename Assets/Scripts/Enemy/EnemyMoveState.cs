using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyMoveState
{
    private PathSearcher _pathSearcher;

    public PathSearcher PathSearcher => _pathSearcher;

    public abstract Vector3 GetNextPosition(Vector3 currentPosition);

    public EnemyMoveState(PathSearcher pathSearcher)
    {
        _pathSearcher = pathSearcher;
    }
}
