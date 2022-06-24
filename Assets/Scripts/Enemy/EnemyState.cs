using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyState
{
    private PathSearcher _pathSearcher;

    public PathSearcher PathSearcher => _pathSearcher;

    public abstract Vector3 GetNextPosition(Transform currentPosition);

    public EnemyState(PathSearcher pathSearcher)
    {
        _pathSearcher = pathSearcher;
    }
}
