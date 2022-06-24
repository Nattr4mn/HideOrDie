using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    private PathSearcher _pathSearcher;

    public abstract Vector3 GetNextPosition(Transform currentPosition);

    public EnemyState(PathSearcher pathSearcher)
    {
        _pathSearcher = pathSearcher;
    }

    public List<Vector2> CreatePath(Vector2 startPoint, Vector2 endPoint)
    {
        float startVectorX = (float)Math.Round(startPoint.x);
        float startVectorY = (float)Math.Round(startPoint.y);
        Vector2 startVector = new Vector2(startVectorX, startVectorY);
        float targetVectorX = (float)Math.Round(endPoint.x);
        float targetVectorY = (float)Math.Round(endPoint.y);
        Vector2 targetVector = new Vector2(targetVectorX, targetVectorY);
        return _pathSearcher.SearchWay(startVector, targetVector);
    }
}
